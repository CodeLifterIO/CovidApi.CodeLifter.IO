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
using Microsoft.EntityFrameworkCore;
using Octokit;
using Slugify;
using Microsoft.Extensions.Logging;
using CovidApi.Settings;
using CovidApi.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace CovidApi.Services
{
    public interface IGithubService
    {
        Task<ApiLimitReport> GetLimitsAsync();
        Task<List<DataFile>> DownloadNewFilesFromGithub();
        Task ParseAndDeleteFile(DataFile df);
        Task StoreDataAndClearMemory();
    }

    public class GithubService : IGithubService
    {
        private Dictionary<string, Country> Countries { get; set; } = new Dictionary<string, Country>();
        private Dictionary<string, Province> Provinces { get; set; } = new Dictionary<string, Province>();
        private Dictionary<string, District> Districts { get; set; } = new Dictionary<string, District>();
        private List<DataPoint> DataPoints { get; set; } = new List<DataPoint>();

        private readonly WebClient _webClient; //NOT INJECTED
        private readonly IGitHubClient _githubClient; //NOT INJECTED

        private readonly ILogger<GithubService> _logger;
        private readonly GithubSettings _githubSettings;
        private readonly ISlugHelper _slugHelper;

        private readonly ICountryRepository _countryRepo;
        private readonly IDataFileRepository _datafileRepo;
        private readonly IDataPointRepository _dataPointRepo;
        private readonly IDistrictRepository _districtRepo;
        private readonly IGeoCoordinateRepository _geoCoordinateRepository;
        private readonly IProvinceRepository _provinceRepo;
        private readonly ITotalRepository _totalRepo;

        
        
        
        


        public GithubService(ILogger<GithubService> logger,
                            IOptionsMonitor<GithubSettings> optionsMonitor,
                            ISlugHelper slugHelper,
                            ICountryRepository countryRepository,
                            IDataFileRepository datafileRepo,
                            IDataPointRepository dataPointRepository,
                            IDistrictRepository districtRepository,
                            IGeoCoordinateRepository geoCoordinateRepository,
                            IProvinceRepository provinceRepository,
                            ITotalRepository totalRepository)
        {
            _logger = logger;
            _githubSettings = optionsMonitor.CurrentValue;
            _slugHelper = slugHelper;

            _githubClient = new GitHubClient(new ProductHeaderValue(_githubSettings.ProductHeaderValue))
            {
                Credentials = new Credentials(_githubSettings.Token),
            };

            _webClient = new WebClient();
            _webClient.Headers.Add(_githubSettings.ProductHeaderValue, _githubSettings.Token);

            _countryRepo = countryRepository;
            _datafileRepo = datafileRepo;
            _dataPointRepo = dataPointRepository;
            _districtRepo = districtRepository;
            _geoCoordinateRepository = geoCoordinateRepository;
            _provinceRepo = provinceRepository;
            _totalRepo = totalRepository;
        }

        public async Task<ApiLimitReport> GetLimitsAsync()
        {
            ApiLimitReport report = new ApiLimitReport();

            var miscellaneousRateLimit = await _githubClient.Miscellaneous.GetRateLimits();

            //  The "core" object provides your rate limit status except for the Search API.
            var coreRateLimit = miscellaneousRateLimit.Resources.Core;

            report.RequestsPerHour = coreRateLimit.Limit;
            report.RemainingRequests = coreRateLimit.Remaining;
            report.LimitResetTime = coreRateLimit.Reset; // UTC time

            return report;
        }

        public async Task<List<DataFile>> DownloadNewFilesFromGithub()
        {
            List<DataFile> files = new List<DataFile>();
            var fileInfos = await _githubClient.Repository.Content
                                                          .GetAllContents(_githubSettings.RepoOwner,
                                                                          _githubSettings.RepoName,
                                                                          _githubSettings.GithubFolderPath);
            foreach (RepositoryContent rc in fileInfos)
            {
                if (rc.Name.EndsWith(".csv") && !(await _datafileRepo.ExistsAsync(rc.Name)))
                {
                    DataFile file = new DataFile()
                    {
                        FileName = rc.Name,
                        FileUrl = rc.DownloadUrl,
                    };
                    files.Add(file);
                    {
                        await _datafileRepo.AddAsync(file);
                    }
                }
            }
            return files;
        }

        public async Task ParseAndDeleteFile(DataFile df)
        {
            await _webClient.DownloadFileTaskAsync(new Uri(df.FileUrl), df.FileName);

            string[] csvRows = File.ReadAllLines(df.FileName);
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
                await GenerateEntriesFromDelimitedFields(df.FileName, headers, row);
            }

            df.RecordsProcessed = csvRows.Length;

            File.Delete($"{df.FileName}");
        }

        public async Task StoreDataAndClearMemory()
        {
            //Add Data to the DB then clear local in memory storage
            await _countryRepo.UpsertRangeAsync(Countries.Values.ToList());
            Countries.Clear();
            await _provinceRepo.UpsertRangeAsync(Provinces.Values.ToList());
            Provinces.Clear();
            await _districtRepo.UpsertRangeAsync(Districts.Values.ToList());
            Districts.Clear();
            await _dataPointRepo.AddRangeAsync(DataPoints);
            DataPoints.Clear();
        }

        private async Task GenerateEntriesFromDelimitedFields(string fileName, string[] headers, string[] row)
        {
            if (headers.Length > row.Length)
            {
                _logger.LogError("Array Mismatch");
                string[] altRow = new string[headers.Length];
                row.CopyTo(altRow, 1);
                row = altRow;
            }

            string fips = string.Empty;
            District district = null;
            Province province = null;
            Country country = null;
            GeoCoordinate geoCoordinate = null;
            double latitude = 0.0;
            double longitude = 0.0;

            DataPoint dataPoint = new DataPoint();

            for (int i = 0; i < headers.Length; i++)
            {
                try
                {
                    switch (headers[i])
                    {
                        case "FIPS":
                            fips = row[i];
                            break;
                        case "Admin2":
                            string name = row[i].Replace("/", ", ");
                            district = new District()
                            {
                                Name = name,
                                SlugId = _slugHelper.GenerateSlug(name),
                                FIPS = fips,
                            };
                            break;
                        case "ProvinceState":
                            string provinceName;
                            if (row[i] != null && row[i].Contains('\"'))
                            {
                                string cleanStr = row[i].Replace("\"", "")
                                                        .Replace("/", ",");
                                string[] splitOnComma = cleanStr.Split(",");
                                provinceName = splitOnComma[1];
                                district = new District()
                                {
                                    Name = splitOnComma[0],
                                    SlugId = _slugHelper.GenerateSlug(splitOnComma[0])
                                };
                            }
                            else if(row[i] != null)
                            {
                                provinceName = row[i];
                                province = new Province()
                                {
                                    Name = provinceName,
                                    SlugId = _slugHelper.GenerateSlug(provinceName),
                                };
                            }
                            break;
                        case "CountryRegion":
                            string countryName = row[i].Replace("/", "")
                                                        .Replace("\"", "");
                            country = new Country()
                            {
                                Name = countryName,
                                SlugId = _slugHelper.GenerateSlug(countryName),
                            };
                            break;
                        case "LastUpdate":
                            dataPoint.LastUpdate = DateTime.Parse(fileName.Replace(".csv", ""));
                            break;
                        case "Lat":
                            latitude = ParseDouble(row[i]);
                            break;
                        case "Latitude":
                            latitude = ParseDouble(row[i]);
                            break;
                        case "Long":
                            longitude = ParseDouble(row[i]);
                            break;
                        case "Longitude":
                            longitude = ParseDouble(row[i]);
                            break;
                        case "Confirmed":
                            dataPoint.Confirmed = ParseInt(row[i]);
                            break;
                        case "Deaths":
                            dataPoint.Deaths = ParseInt(row[i]);
                            break;
                        case "Recovered":
                            dataPoint.Recovered = ParseInt(row[i]);
                            break;
                        case "Active":
                            dataPoint.Active = ParseInt(row[i]);
                            break;
                        case "CombinedKey":
                            dataPoint.CombinedKey = row[i];
                            break;
                        case "IncidenceRate":
                            dataPoint.IncidenceRate = ParseDouble(row[i]);
                            break;
                        case "CaseFatalityRatio":
                            dataPoint.CaseFatalityRatio = ParseDouble(row[i]);
                            break;
                        default:
                            _logger.LogInformation($"{fileName} - {row[i]} Rows:{row.Length} Headers: {headers.Length}");
                            break;
                    }
                }
                catch (Exception exc)
                {
                    _logger.LogError(exc.Message);
                }
            }
            dataPoint.SourceFile = fileName.Replace(".csv", "");
            if (latitude != 0.0 && longitude != 0.0) geoCoordinate = new GeoCoordinate() { Latitude = latitude, Longitude = longitude };
            await ProcessDataPointAsync(dataPoint, geoCoordinate, district, province, country);
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

        private async Task ProcessDataPointAsync(DataPoint dp, GeoCoordinate geo, District district,
                                    Province province, Country country)
        {
            if(geo != null)
            {
                await _geoCoordinateRepository.UpsertAsync(geo);
            }

            if (country != null)
            {
                if (district == null && province == null && geo != null)
                {
                    country.GeoCoordinateId = geo.Id;
                }
                dp.CountrySlugId = country.SlugId;
                Countries[country.SlugId] = country;
            }

            if (province != null)
            {
                if (district == null && geo != null)
                {
                    province.GeoCoordinateId = geo.Id;
                }
                province.CountrySlugId = country?.SlugId;
                dp.ProvinceSlugId = province.SlugId;
                Provinces[province.SlugId] = province;
            }

            if (null != district)
            {
                if (geo != null)
                {
                    district.GeoCoordinateId = geo.Id;
                }
                district.CountrySlugId = country?.SlugId;
                district.ProvinceSlugId = province?.SlugId;
                dp.DistrictSlugId = district.SlugId;
                Districts[district.SlugId] = district;
            }

            DataPoints.Add(dp);
        }


    }

}
