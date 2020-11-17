//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Threading.Tasks;
//using CovidApi.Services;
//using CovidApi.Data;
//using CovidApi.Models;
//using Microsoft.EntityFrameworkCore;
//using Octokit;
//using Slugify;
//using Microsoft.Extensions.Logging;
//using CovidApi.Settings;

//namespace CovidApi.Services
//{
//    public interface IGithubService
//    {
//        Task DownloadAllFilesAsync();
//        Task<List<GithubDataFile>> GetListOfFilesAsync();
//        //int ParseAndDeleteFile(string path, string fileName);
//        Task<ApiLimitReport> ReportAPILimitsAsync();
//    }

//    public class GithubService : IGithubService
//    {
//        private readonly GithubSettings _githubSettings;
//        private ILogger<GithubService> _logger;
//        private GitHubClient _githubClient;
//        private WebClient _webClient;
//        private IReadOnlyList<RepositoryContent> GlobalDataFileInfos { get; set; }
//        private ISlugHelper _slugHelper;

//        public GithubService(ILogger<GithubService> logger,
//                             GithubSettings githubSettings)
//                             //GitHubClient githubClient,
//                             //WebClient webClient)
//                             //ISlugHelper slugHelper)
//        {
//            _logger = logger;
//            //_githubSettings = githubSettings;
//            //_githubClient = new GitHubClient(new ProductHeaderValue(_githubSettings.Token));
//            //_webClient = webClient;
//            //_slugHelper = slugHelper;

//            //configure Services
//            //_githubClient.Credentials = new Credentials(_githubSettings.Token);
//        }

//        public async Task DownloadAllFilesAsync()
//        {
//            List<GithubDataFile> gFiles = new List<GithubDataFile>();

//            gFiles = await GetListOfFilesAsync();

//            foreach (GithubDataFile gFile in gFiles)
//            {
//                _logger.LogInformation($"File: {gFile.FileName}");
//                //    if (gFile.FileName == startFile || string.IsNullOrEmpty(startFile))
//                //    {
//                //        Update.StartRun(gFile);
//                //    }

//                //    if (Update.IsStarted == true && startFile != gFile.FileName)
//                //    {
//                //        Update.StartFile(gFile);
//                //        int recordCount = ParseAndDeleteFile(gFile.DownloadUrl, gFile.FileName);
//                //        Update.FinishFile(recordCount);
//                //    }
//            }

//            //Update.Finish();
//            //_logger.LogMessage($"COVIDAP -> SUCCESS - UP TO DATE. File: {Update.CurrentUpdateState.LastCompletedFileName}");
//        }

//        public async Task<ApiLimitReport> ReportAPILimitsAsync()
//        {
//            ApiLimitReport report = new ApiLimitReport();

//            var miscellaneousRateLimit = await _githubClient.Miscellaneous.GetRateLimits();

//            //  The "core" object provides your rate limit status except for the Search API.
//            var coreRateLimit = miscellaneousRateLimit.Resources.Core;

//            report.RequestsPerHour = coreRateLimit.Limit;
//            report.RemainingRequests = coreRateLimit.Remaining;
//            report.LimitResetTime = coreRateLimit.Reset; // UTC time

//            return report;
//        }

//        public async Task<List<Models.GithubDataFile>> GetListOfFilesAsync()
//        {
//            List<Models.GithubDataFile> files = new List<Models.GithubDataFile>();
//            GlobalDataFileInfos = await _githubClient.Repository
//                .Content
//                .GetAllContents(_githubSettings.RepoOwner, _githubSettings.RepoName, _githubSettings.GithubFolderPath);

//            foreach (RepositoryContent rc in GlobalDataFileInfos)
//            {
//                if (rc.Name.EndsWith(".csv"))
//                {
//                    Models.GithubDataFile file = new Models.GithubDataFile()
//                    {
//                        FileName = rc.Name,
//                        DownloadUrl = rc.DownloadUrl,
//                    };
//                    files.Add(file);
//                }
//            }

//            return files;
//        }


//        //public int ParseAndDeleteFile(string path, string fileName)
//        //{
//        //    _webClient.DownloadFile(new Uri(path), fileName);

//        //    string[] csvRows = File.ReadAllLines(fileName);
//        //    string[] headers = csvRows[0].Replace("/", "")
//        //                                .Replace("_", "")
//        //                                .Replace("-", "")
//        //                                .Replace(" ", "")
//        //                                .Split(',');
//        //    for (int i = 1; i < csvRows.Length; i++)
//        //    {
//        //        string[] row = csvRows[i]
//        //                                .Replace(", ", "/")
//        //                                .Split(',');
//        //        //GenerateEntryFromDelimitedFields(fileName, headers, row);
//        //        //StoredProcedure.SummarizeEntities();
//        //        //StoredProcedure.GenerateDatabaseBackup();
//        //    }

//        //    File.Delete($"{fileName}");

//        //    return csvRows.Length;
//        //}


//        ///// <summary>
//        ///// TODO Replace with cleaner library implementation
//        ///// </summary>
//        ///// <param name="fileName"></param>
//        ///// <param name="headers"></param>
//        ///// <param name="row"></param>
//        ///// <returns></returns>
//        //private void GenerateEntryFromDelimitedFields(string fileName, string[] headers, string[] row)
//        //{
//        //    if (headers.Length > row.Length)
//        //    {
//        //        Log.LogMessage("Array Mismatch", LogLevels.Warning);
//        //        string[] altRow = new string[headers.Length];
//        //        row.CopyTo(altRow, 1);
//        //        row = altRow;
//        //    }

//        //    string fips = string.Empty;
//        //    District district = null;
//        //    Province province = null;
//        //    Country country = null;
//        //    GeoCoordinate geoCoordinate = null;
//        //    double latitude = 0.0;
//        //    double longitude = 0.0;

//        //    DataPoint dataPoint = new DataPoint();

//        //    for (int i = 0; i < headers.Length; i++)
//        //    {
//        //        try
//        //        {
//        //            switch (headers[i])
//        //            {
//        //                case "FIPS":
//        //                    fips = row[i];
//        //                    break;
//        //                case "Admin2":
//        //                    district = new District(row[i].Replace("/", ", "), fips);
//        //                    break;
//        //                case "ProvinceState":
//        //                    string provinceName;
//        //                    if (row[i] != null && row[i].Contains('\"'))
//        //                    {
//        //                        string cleanStr = row[i].Replace("\"", "")
//        //                                                .Replace("/", ",");
//        //                        string[] splitOnComma = cleanStr.Split(",");
//        //                        provinceName = splitOnComma[1];
//        //                        district = new District(splitOnComma[0]);
//        //                    }
//        //                    else
//        //                    {
//        //                        provinceName = row[i];
//        //                    }
//        //                    province = new Province(provinceName);
//        //                    break;
//        //                case "CountryRegion":
//        //                    country = new Country(row[i].Replace("/", "")
//        //                                                .Replace("\"", ""));
//        //                    break;
//        //                case "LastUpdate":
//        //                    dataPoint.LastUpdate = DateTime.Parse(fileName.Replace(".csv", ""));
//        //                    break;
//        //                case "Lat":
//        //                    latitude = ParseDouble(row[i]);
//        //                    break;
//        //                case "Latitude":
//        //                    latitude = ParseDouble(row[i]);
//        //                    break;
//        //                case "Long":
//        //                    longitude = ParseDouble(row[i]);
//        //                    break;
//        //                case "Longitude":
//        //                    longitude = ParseDouble(row[i]);
//        //                    break;
//        //                case "Confirmed":
//        //                    dataPoint.Confirmed = ParseInt(row[i]);
//        //                    break;
//        //                case "Deaths":
//        //                    dataPoint.Deaths = ParseInt(row[i]);
//        //                    break;
//        //                case "Recovered":
//        //                    dataPoint.Recovered = ParseInt(row[i]);
//        //                    break;
//        //                case "Active":
//        //                    dataPoint.Active = ParseInt(row[i]);
//        //                    break;
//        //                case "CombinedKey":
//        //                    dataPoint.CombinedKey = row[i];
//        //                    break;
//        //                case "IncidenceRate":
//        //                    dataPoint.IncidenceRate = ParseDouble(row[i]);
//        //                    break;
//        //                case "CaseFatalityRatio":
//        //                    dataPoint.CaseFatalityRatio = ParseDouble(row[i]);
//        //                    break;
//        //                default:
//        //                    Log.LogMessage($"{fileName} - {row[i]} Rows:{row.Length} Headers: {headers.Length}");
//        //                    break;
//        //            }
//        //        }
//        //        catch (Exception exc)
//        //        {
//        //            Log.LogMessage(exc.Message, LogLevels.Warning);
//        //        }
//        //    }
//        //    dataPoint.SourceFile = fileName.Replace(".csv", "");
//        //    if (latitude != 0.0 && longitude != 0.0) geoCoordinate = new GeoCoordinate() { Latitude = latitude, Longitude = longitude};
//        //    ProcessDataPoint(dataPoint, geoCoordinate, district, province, country);
//        //}



//        //public void ProcessDataPoint(DataPoint dp, GeoCoordinate geo, District district,
//        //                            Province province, Country country)
//        //{
//        //    if (country != null)
//        //    {
//        //        if (district == null && province == null && geo != null)
//        //        {
//        //            geo.GeoSlug = country.GeoSlug;
//        //        }
//        //        dp.CountrySlugId = country.SlugId;
//        //        Update.Countries[country.SlugId] = country;
//        //    }

//        //    if (province != null)
//        //    {
//        //        province.CountrySlugId = country.SlugId;
//        //        if (district == null && geo != null)
//        //        {
//        //            geo.GeoSlug = province.GeoSlug;
//        //        }
//        //        province.CountrySlugId = country?.SlugId;
//        //        dp.ProvinceSlugId = province.SlugId;
//        //        Update.Provinces[province.SlugId] = province;
//        //    }

//        //    if (null != district)
//        //    {
//        //        if (geo != null)
//        //        {
//        //            geo.GeoSlug = district.GeoSlug;
//        //        }
//        //        district.CountrySlugId = country?.SlugId;
//        //        district.ProvinceSlugId = province?.SlugId;
//        //        Update.Districts[district.SlugId] = district;
//        //    }

//        //    if (null != geo)
//        //    {
//        //        Update.GeoCoordinates[geo.GeoSlug] = geo;
//        //    }

//        //    Update.DataPoints.Add(dp);
//        //}

//        private double ParseDouble(string source)
//        {
//            if (!string.IsNullOrWhiteSpace(source))
//            {
//                return double.Parse(source);
//            }
//            return 0;
//        }

//        private int ParseInt(string s)
//        {
//            if (!string.IsNullOrWhiteSpace(s))
//            {
//                return int.Parse(s);
//            }
//            return 0;
//        }

//    }
//}

