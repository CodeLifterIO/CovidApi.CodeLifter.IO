using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeLifter.Logging.Loggers;
using CovidApi.Models;
using CovidApi.Repositories;
using CovidApi.Services;
using CovidApi.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Octokit;

namespace CovidApi.Controllers
{
    [ApiController]
    public class AdminController : Controller
    {
        private ILogger<AdminController> _logger;
        private readonly GithubSettings _githubSettings;
        private IGitHubClient _githubClient;
        private IDataFileRepository _datafileRepo;

        public AdminController(ILogger<AdminController> logger,
                               IOptionsMonitor<GithubSettings> optionsMonitor,
                               IDataFileRepository datafileRepo)
        {
            _logger = logger;
            _githubSettings = optionsMonitor.CurrentValue;
            _githubClient = new GitHubClient(new ProductHeaderValue(_githubSettings.ProductHeaderValue))
            {
                Credentials = new Credentials(_githubSettings.Token),
            };
            _datafileRepo = datafileRepo;
        }

        [HttpGet("[controller]/[action]")]
        public async Task<IActionResult> Limits()
        {
            ApiLimitReport report = new ApiLimitReport();

            var miscellaneousRateLimit = await _githubClient.Miscellaneous.GetRateLimits();

            //  The "core" object provides your rate limit status except for the Search API.
            var coreRateLimit = miscellaneousRateLimit.Resources.Core;

            report.RequestsPerHour = coreRateLimit.Limit;
            report.RemainingRequests = coreRateLimit.Remaining;
            report.LimitResetTime = coreRateLimit.Reset; // UTC time

            return Ok(report);
        }

        [HttpGet("[controller]/[action]")]
        public async Task<IActionResult> FileList()
        {
            List<DataFile> files = new List<DataFile>();
            var fileInfos = await _githubClient.Repository.Content
                                                          .GetAllContents(_githubSettings.RepoOwner, 
                                                                          _githubSettings.RepoName, 
                                                                          _githubSettings.GithubFolderPath);
            foreach (RepositoryContent rc in fileInfos)
            {
                if (rc.Name.EndsWith(".csv"))
                {
                    DataFile file = new DataFile()
                    {
                        FileName = rc.Name,
                        FileUrl = rc.DownloadUrl,
                    };
                    files.Add(file);
                }
            }

            return Ok(files);
        }

        [HttpGet("[controller]/[action]")]
        public async Task<IActionResult> UpdateFileList()
        {
            var fileInfos = await _githubClient.Repository.Content
                                                          .GetAllContents(_githubSettings.RepoOwner,
                                                                          _githubSettings.RepoName,
                                                                          _githubSettings.GithubFolderPath);
            foreach (RepositoryContent rc in fileInfos)
            {
                if (rc.Name.EndsWith(".csv"))
                {
                    DataFile file = new DataFile()
                    {
                        FileName = rc.Name,
                        FileUrl = rc.DownloadUrl,
                    };
                    await _datafileRepo.AddAsync(file);
                }
            }

            return Ok(await _datafileRepo.GetAllAsync());
        }

        //[HttpGet]
        //[Route("[controller]/{slug}/[action]")]
        //public async Task<IActionResult> Process()
        //{
        //    await _githubService.DownloadAllFilesAsync();
        //    return new OkResult();
        //}

    }
}
