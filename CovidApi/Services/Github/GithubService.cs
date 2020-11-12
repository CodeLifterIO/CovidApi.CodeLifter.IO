using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CovidApi.Services;
using CovidApi.Data;
using CovidApi.Models;
using CodeLifter.Logging;
using CodeLifter.Logging.Loggers;
using Microsoft.EntityFrameworkCore;
using Octokit;
using Slugify;

namespace CovidApi.Services
{
    public class StdOutLogger : ILogger
    {
        public void LogEntry(string message, LogLevels level = LogLevels.Trace)
        {
            Console.Out.WriteLine(message);
        }
    }


    public class GithubService
    {
        //github info
        public const string GithubFolderPath = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_daily_reports/";
        private const string ProductHeaderValue = "CodeLifter-Covid-Parser";

        //TODO: Needs cleaned up
        //services
        private LogRunner Log;
        private GitHubClient Client { get; set; }
        WebClient WebClient { get; set; }
        private IReadOnlyList<RepositoryContent> GlobalDataFileInfos { get; set; }

        //Collection Services 
        //DataUpdateService Update { get; set; }

        //Helper tools
        SlugHelper Slugger => new SlugHelper();

        public GithubService(string token, List<Entry> entries = null, LogRunner log = null)
        {
            if (log == null)
            {
                Log = new LogRunner();
                Log.Loggers.Clear();
            }
            else
            {
                Log = log;
            }
            Log.AddLogger(new StdOutLogger());

            WebClient = new WebClient();

            Client = new GitHubClient(new ProductHeaderValue(ProductHeaderValue));

            //Update = new DataUpdateService();

            if (!string.IsNullOrWhiteSpace(token))
            {
                var tokenAuth = new Credentials(token);
                Client.Credentials = tokenAuth;
                Log.LogMessage("*** Instantiating Github Service ***");
            }
            else if(!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("GITHUB_TOKEN")))
            {
                var tokenAuth = new Credentials(Environment.GetEnvironmentVariable("GITHUB_TOKEN")); // NOTE: not real token
                Client.Credentials = tokenAuth;
                Log.LogMessage("*** Instantiating Github Service with Auth ***");
            }
        }

        public async Task DownloadAllFiles(string startFile = null)
        {
            //if (string.IsNullOrEmpty(startFile))
            //{
            //    using (var context = new CovidContext())
            //    {
            //        var fileList = await context.DataFiles.ToListAsync();
                    
            //        if(fileList.Count > 0)
            //            startFile = fileList.Last()?.FileName;
            //    }
            //}


            List<GithubDataFile> gFiles = new List<GithubDataFile>();

            gFiles = await GetListOfFiles("CSSEGISandData",
                                                "COVID-19",
                                                "csse_covid_19_data/csse_covid_19_daily_reports");

            foreach (GithubDataFile gFile in gFiles)
            {
                if (gFile.FileName == startFile || string.IsNullOrEmpty(startFile))
                {
                    Update.StartRun(gFile);
                }

                if (Update.IsStarted == true && startFile != gFile.FileName)
                {
                    Update.StartFile(gFile);
                    int recordCount = ParseAndDeleteFile(gFile.DownloadUrl, gFile.FileName);
                    Update.FinishFile(recordCount);
                }
            }

            Update.Finish();
            Log.LogMessage($"COVIDAP -> SUCCESS - UP TO DATE. File: {Update.CurrentUpdateState.LastCompletedFileName}");
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

        public async Task<List<Models.GithubDataFile>> GetListOfFiles(string repoOwner, string repoName, string folderPath)
        {
            List<Models.GithubDataFile> files = new List<Models.GithubDataFile>();
            GlobalDataFileInfos = await Client.Repository
                .Content
                .GetAllContents(repoOwner, repoName, folderPath);

            foreach (RepositoryContent rc in GlobalDataFileInfos)
            {
                if (rc.Name.EndsWith(".csv"))
                {
                    Models.GithubDataFile file = new Models.GithubDataFile()
                    {
                        FileName = rc.Name,
                        DownloadUrl = rc.DownloadUrl,
                    };
                    files.Add(file);
                }
            }

            return files;
        }


        public int ParseAndDeleteFile(string path, string fileName)
        {
            WebClient.DownloadFile(new Uri(path), fileName);

            string[] csvRows = File.ReadAllLines(fileName);
            string[] headers = csvRows[0].Replace("/", "")
                                        .Replace("_", "")
                                        .Replace("-", "")
                                        .Replace(" ", "")
                                        .Split(',');
            for (int i = 1; i < csvRows.Length; i++)
            {
                string[] row = csvRows[i]
                                        .Replace(", ", "/")
                                        .Split(',');
                //GenerateEntryFromDelimitedFields(fileName, headers, row);
                //StoredProcedure.SummarizeEntities();
                //StoredProcedure.GenerateDatabaseBackup();
            }

            File.Delete($"{fileName}");

            return csvRows.Length;
        }


        ///// <summary>
        ///// TODO Replace with cleaner library implementation
        ///// </summary>
        ///// <param name="fileName"></param>
        ///// <param name="headers"></param>
        ///// <param name="row"></param>
        ///// <returns></returns>
        //private void GenerateEntryFromDelimitedFields(string fileName, string[] headers, string[] row)
        //{
        //    if (headers.Length > row.Length)
        //    {
        //        Log.LogMessage("Array Mismatch", LogLevels.Warning);
        //        string[] altRow = new string[headers.Length];
        //        row.CopyTo(altRow, 1);
        //        row = altRow;
        //    }

        //    string fips = string.Empty;
        //    District district = null;
        //    Province province = null;
        //    Country country = null;
        //    GeoCoordinate geoCoordinate = null;
        //    double latitude = 0.0;
        //    double longitude = 0.0;

        //    DataPoint dataPoint = new DataPoint();

        //    for (int i = 0; i < headers.Length; i++)
        //    {
        //        try
        //        {
        //            switch (headers[i])
        //            {
        //                case "FIPS":
        //                    fips = row[i];
        //                    break;
        //                case "Admin2":
        //                    district = new District(row[i].Replace("/", ", "), fips);
        //                    break;
        //                case "ProvinceState":
        //                    string provinceName;
        //                    if (row[i] != null && row[i].Contains('\"'))
        //                    {
        //                        string cleanStr = row[i].Replace("\"", "")
        //                                                .Replace("/", ",");
        //                        string[] splitOnComma = cleanStr.Split(",");
        //                        provinceName = splitOnComma[1];
        //                        district = new District(splitOnComma[0]);
        //                    }
        //                    else
        //                    {
        //                        provinceName = row[i];
        //                    }
        //                    province = new Province(provinceName);
        //                    break;
        //                case "CountryRegion":
        //                    country = new Country(row[i].Replace("/", "")
        //                                                .Replace("\"", ""));
        //                    break;
        //                case "LastUpdate":
        //                    dataPoint.LastUpdate = DateTime.Parse(fileName.Replace(".csv", ""));
        //                    break;
        //                case "Lat":
        //                    latitude = ParseDouble(row[i]);
        //                    break;
        //                case "Latitude":
        //                    latitude = ParseDouble(row[i]);
        //                    break;
        //                case "Long":
        //                    longitude = ParseDouble(row[i]);
        //                    break;
        //                case "Longitude":
        //                    longitude = ParseDouble(row[i]);
        //                    break;
        //                case "Confirmed":
        //                    dataPoint.Confirmed = ParseInt(row[i]);
        //                    break;
        //                case "Deaths":
        //                    dataPoint.Deaths = ParseInt(row[i]);
        //                    break;
        //                case "Recovered":
        //                    dataPoint.Recovered = ParseInt(row[i]);
        //                    break;
        //                case "Active":
        //                    dataPoint.Active = ParseInt(row[i]);
        //                    break;
        //                case "CombinedKey":
        //                    dataPoint.CombinedKey = row[i];
        //                    break;
        //                case "IncidenceRate":
        //                    dataPoint.IncidenceRate = ParseDouble(row[i]);
        //                    break;
        //                case "CaseFatalityRatio":
        //                    dataPoint.CaseFatalityRatio = ParseDouble(row[i]);
        //                    break;
        //                default:
        //                    Log.LogMessage($"{fileName} - {row[i]} Rows:{row.Length} Headers: {headers.Length}");
        //                    break;
        //            }
        //        }
        //        catch (Exception exc)
        //        {
        //            Log.LogMessage(exc.Message, LogLevels.Warning);
        //        }
        //    }
        //    dataPoint.SourceFile = fileName.Replace(".csv", "");
        //    if (latitude != 0.0 && longitude != 0.0) geoCoordinate = new GeoCoordinate() { Latitude = latitude, Longitude = longitude};
        //    ProcessDataPoint(dataPoint, geoCoordinate, district, province, country);
        //}



        //public void ProcessDataPoint(DataPoint dp, GeoCoordinate geo, District district,
        //                            Province province, Country country)
        //{
        //    if (country != null)
        //    {
        //        if (district == null && province == null && geo != null)
        //        {
        //            geo.GeoSlug = country.GeoSlug;
        //        }
        //        dp.CountrySlugId = country.SlugId;
        //        Update.Countries[country.SlugId] = country;
        //    }

        //    if (province != null)
        //    {
        //        province.CountrySlugId = country.SlugId;
        //        if (district == null && geo != null)
        //        {
        //            geo.GeoSlug = province.GeoSlug;
        //        }
        //        province.CountrySlugId = country?.SlugId;
        //        dp.ProvinceSlugId = province.SlugId;
        //        Update.Provinces[province.SlugId] = province;
        //    }

        //    if (null != district)
        //    {
        //        if (geo != null)
        //        {
        //            geo.GeoSlug = district.GeoSlug;
        //        }
        //        district.CountrySlugId = country?.SlugId;
        //        district.ProvinceSlugId = province?.SlugId;
        //        Update.Districts[district.SlugId] = district;
        //    }

        //    if (null != geo)
        //    {
        //        Update.GeoCoordinates[geo.GeoSlug] = geo;
        //    }

        //    Update.DataPoints.Add(dp);
        //}

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

    }
}

