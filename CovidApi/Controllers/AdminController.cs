using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private IDataFileRepository _datafileRepo;
        private IGithubService _gitService;

        public AdminController(ILogger<AdminController> logger,
                               IDataFileRepository datafileRepo,
                               IGithubService githubService)
        {
            _gitService = githubService;
            _logger = logger;
            _datafileRepo = datafileRepo;
        }

        [HttpGet("[controller]/[action]")]
        public async Task<IActionResult> Limits()
        {
            var report = await _gitService.GetLimitsAsync();

            return Ok(report);
        }

        //[HttpGet("[controller]/[action]")]
        //public async Task<IActionResult> Download()
        //{
        //    var report = await _gitService.DownloadNewFilesFromGithub();

        //    return Ok(report);
        //}

        [HttpGet("[controller]/[action]")]
        public async Task<IActionResult> Index()
        {
            var report = await _datafileRepo.GetAllAsync();

            return Ok(report);
        }
    }
}
