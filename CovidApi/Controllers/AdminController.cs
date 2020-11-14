using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CovidApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CovidApi.Controllers
{
    public class AdminController : Controller
    {
        private IGithubService _githubService;

        public AdminController(IGithubService githubService)
        {
            _githubService = githubService;
        }

        // GET: AdminController
        public async Task<IActionResult> Limits()
        {
            var report = await _githubService.ReportAPILimits();
            return Ok(report);
        }

        [HttpGet]
        [Route("[controller]/{slug}/[action]")]
        public async Task<IActionResult> Process()
        {
            await _githubService.DownloadAllFiles();
            return new OkResult();
        }

    }
}
