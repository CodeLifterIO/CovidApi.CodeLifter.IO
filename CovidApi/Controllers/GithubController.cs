using System;
using System.Threading.Tasks;
using CovidApi.Infrastructure;
using CovidApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CovidApi.Controllers
{
    public class GithubController : BaseController
    {
        private static GithubService _service { get; set; }
        public static GithubService Service
        {
            get
            {
                if (_service == null)
                    _service = new GithubService(Environment.GetEnvironmentVariable("GITHUB_AUTH_TOKEN"));

                return _service;
            }
        }

        [HttpGet]
        [Route("[controller]/{slug}/[action]")]
        public async Task<IActionResult> Process()
        {
            await Service.DownloadAllFiles();
            return new OkResult();
        }
    }
}
