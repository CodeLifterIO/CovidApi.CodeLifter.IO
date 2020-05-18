using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using CodeLifter.Logging;
using CodeLifter.Logging.Loggers;
using Octokit;
using CodeLifter.Covid19.Data;
using CodeLifter.Covid19.Data.Models;
using CovidApi.CodeLifter.IO.Management.Models;

namespace CovidApi.CodeLifter.IO.Management.Services
{
    public class GithubService
    {
        private const string ProductHeaderValue = "CodeLifter-Covid-Parser";
        private LogRunner Log;
        private GitHubClient Client { get; set; }
        WebClient WebClient { get; set; }
        private IReadOnlyList<RepositoryContent> GlobalDataFileInfos { get; set; }
        public List<Entry> Entries { get; private set; }

        public GithubService(string token, List<Entry> entries = null, LogRunner log = null)
        {
            if (entries == null)
            {
                Entries = new List<Entry>();
            }
            else
            {
                Entries = entries;
            }

            if (log == null)
            {
                Log = new LogRunner();
                Log.AddLogger(new ConsoleLogger());
            }
            else
            {
                Log = log;
            }
            Log.AddLogger(new DebugLogger());

            WebClient = new WebClient();

            Client = new GitHubClient(new ProductHeaderValue(ProductHeaderValue));

            if (!string.IsNullOrWhiteSpace(token))
            {
                var tokenAuth = new Credentials(token); // NOTE: not real token
                Client.Credentials = tokenAuth;
            }
        }

        public async Task<ApiLimitReport> ReportAPILimits()
        {
            ApiLimitReport report = new ApiLimitReport();

            var miscellaneousRateLimit = await Client.Miscellaneous.GetRateLimits();

            //  The "core" object provides your rate limit status except for the Search API.
            var coreRateLimit = miscellaneousRateLimit.Resources.Core;

            report.RequestsPerHour = coreRateLimit.Limit;
            report.RemainingRequests = coreRateLimit.Remaining;
            report.LimitResetTime = coreRateLimit.Reset; // UTC time

            return report;
        }

        public async Task<List<DataFile>> GetListOfFiles(string repoOwner, string repoName, string folderPath)
        {
            List<DataFile> files = new List<DataFile>();
            GlobalDataFileInfos = await Client.Repository
                .Content
                .GetAllContents(repoOwner, repoName, folderPath);

            foreach (RepositoryContent rc in GlobalDataFileInfos)
            {
                if (rc.Name.EndsWith(".csv"))
                {
                    DataFile file = new DataFile()
                    {
                        FileName = rc.Name,
                        DownloadUrl = rc.DownloadUrl,
                    };
                    files.Add(file);
                }
            }

            return files;
        }

        public async Task ParseAndDeleteFile(DataFile file)
        {
            await ParseAndDeleteFile(file.DownloadUrl, file.FileName);
        }

        public async Task ParseAndDeleteFile(string path, string fileName)
        {
            await WebClient.DownloadFileTaskAsync(new Uri(path), fileName);

            string[] csvRows = File.ReadAllLines(fileName);
            string[] headers = csvRows[0].Replace("/", "")
                                        .Replace("_", "")
                                        .Replace(" ", "")
                                        .Split(',');
            for (int i = 1; i < csvRows.Length; i++)
            {
                string[] row = csvRows[i]
                                            .Replace(", ", "/")
                                            .Split(',');
                Entries.Add(GenerateEntryFromDelimitedFields(fileName, headers, row));
            }

            File.Delete($"{fileName}");
        }


        /// <summary>
        /// TODO Replace with cleaner library implementation
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="headers"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private Entry GenerateEntryFromDelimitedFields(string fileName, string[] headers, string[] row)
        {
            if (headers.Length > row.Length)
            {
                Log.LogMessage("Array Mismatch", LogLevels.Warning);
                string[] altRow = new string[headers.Length];
                row.CopyTo(altRow, 1);
                row = altRow;
            }

            Entry entry = new Entry();

            for (int i = 0; i < headers.Length; i++)
            {
                try
                {
                    switch (headers[i])
                    {
                        case "FIPS":
                            entry.FIPS = row[i];
                            break;
                        case "Admin2":
                            entry.Admin2 = row[i].Replace("/", ", ");
                            break;
                        case "ProvinceState":
                            if (row[i].Contains('\"'))
                            {
                                string cleanStr = row[i].Replace("\"", "")
                                                        .Replace("/", ",");
                                string[] splitOnComma = cleanStr.Split(",");
                                entry.ProvinceState = splitOnComma[1];
                                entry.Admin2 = splitOnComma[0];
                            }
                            else
                            {
                                entry.ProvinceState = row[i];
                            }
                            break;
                        case "CountryRegion":
                            entry.CountryRegion = row[i].Replace("/", "")
                                                        .Replace("\"", "");
                            break;
                        case "LastUpdate":
                            entry.LastUpdate = DateTime.Parse(fileName.Replace(".csv", ""));
                            break;
                        case "Lat":
                            entry.Lat = ParseDouble(row[i]);
                            break;
                        case "Latitude":
                            entry.Lat = ParseDouble(row[i]);
                            break;
                        case "Long":
                            entry.Long = ParseDouble(row[i]);
                            break;
                        case "Longitude":
                            entry.Long = ParseDouble(row[i]);
                            break;
                        case "Confirmed":
                            entry.Confirmed = ParseInt(row[i]);
                            break;
                        case "Deaths":
                            entry.Deaths = ParseInt(row[i]);
                            break;
                        case "Recovered":
                            entry.Recovered = ParseInt(row[i]);
                            break;
                        case "Active":
                            entry.Active = ParseInt(row[i]);
                            break;
                        case "CombinedKey":
                            entry.CombinedKey = row[i];
                            break;
                        default:
                            Log.LogMessage($"{fileName} - {row[i]} Rows:{row.Length} Headers: {headers.Length}");
                            break;
                    }
                }
                catch (Exception exc)
                {
                    Log.LogMessage(exc.Message, LogLevels.Warning);
                }
            }
            entry.SourceFile = fileName;
            return entry;
        }

        private double ParseDouble(string source)
        {
            if (!string.IsNullOrWhiteSpace(source))
            {
                return double.Parse(source);
            }
            return 0;
        }

        private int ParseInt(string s)
        {
            if (!string.IsNullOrWhiteSpace(s))
            {
                return int.Parse(s);
            }
            return 0;
        }

        public async Task<DataCollectionStatistic> SaveEntriesToDataModel(string fileName)
        {
            DataCollectionStatistic stat = new DataCollectionStatistic()
            {
                RecordsProcessed = Entries.Count,
                LastRunStarted = DateTime.Now,
                FileName = fileName
            };
            DataCollectionStatistic.Insert(stat);

            int i = 0;
            foreach (Entry entry in Entries)
            {
                ProcessEntry(entry);
                i++;
                if ((i % 100) == 0)
                {
                    Log.LogMessage($"{i} / {Entries.Count}  -  {entry.ToString()}");
                }
            }

            stat.LastRunCompleted = DateTime.Now;
            stat.RecordsProcessed = i;
            DataCollectionStatistic.Update(stat);
            Entries.Clear();

            Log.LogMessage(stat.ToString(), LogLevels.Trace);

            return stat;
        }

        public void ProcessEntry(Entry entry)
        {
            DataPoint dp = new DataPoint();

            dp.LastUpdate = entry.LastUpdate;
            dp.Confirmed = entry.Confirmed;
            dp.Deaths = entry.Deaths;
            dp.Recovered = entry.Recovered;
            dp.Active = entry.Active;
            dp.CombinedKey = entry.CombinedKey;
            dp.SourceFile = entry.SourceFile.Replace(".csv", "");

            GeoCoordinate geo = UpsertGeo(entry);
            Country country = UpsertCountry(entry);
            Province province = UpsertProvince(entry, country);
            District district = UpsertDistrict(entry, country, province);

            AssignGeoCoordinateAppropriately(geo, country, province, district);

            if (null != country)
                dp.CountryId = country.Id;

            if (null != province)
                dp.ProvinceId = province.Id;

            if (null != district)
                dp.DistrictId = district.Id;

            DataPoint.Upsert(dp);
        }

        public void AssignGeoCoordinateAppropriately(GeoCoordinate geo, Country country, Province province, District district)
        {
            if (null == geo || geo.Id == 0)
            {
                return;
            }

            if (null != district && district.Id != 0)
            {
                district.GeoCoordinateId = geo.Id;
                District.Update(district);
            }
            else if (null != province && province.Id != 0)
            {
                province.GeoCoordinateId = geo.Id;
                Province.Update(province);
            }
            else if (null != country && country.Id != 0)
            {
                country.GeoCoordinateId = geo.Id;
                Country.Update(country);
            }
            return;
        }

        public GeoCoordinate UpsertGeo(Entry entry)
        {
            GeoCoordinate geo = new GeoCoordinate()
            {
                Latitude = entry.Lat,
                Longitude = entry.Long
            };
            return GeoCoordinate.Upsert(geo);
        }

        public Country UpsertCountry(Entry entry)
        {
            Country country = new Country()
            {
                Name = entry.CountryRegion
            };
            return Country.Upsert(country);
        }

        public Province UpsertProvince(Entry entry, Country country)
        {
            if (string.IsNullOrWhiteSpace(entry.ProvinceState))
            {
                return null;
            }
            Province province = new Province()
            {
                Name = entry.ProvinceState
            };
            if (null != country)
            {
                province.CountryId = country.Id;
            }
            return Province.Upsert(province);
        }

        public District UpsertDistrict(Entry entry, Country country, Province province)
        {
            if (string.IsNullOrWhiteSpace(entry.ProvinceState) && string.IsNullOrWhiteSpace(entry.FIPS))
            {
                return null;
            }
            District district = new District()
            {
                Name = entry.Admin2,
                FIPS = entry.FIPS
            };
            if (null != province)
            {
                district.ProvinceId = province.Id;
            }
            if (null != country)
            {
                district.CountryId = country.Id;
            }
            return District.Upsert(district);
        }
    }
}

