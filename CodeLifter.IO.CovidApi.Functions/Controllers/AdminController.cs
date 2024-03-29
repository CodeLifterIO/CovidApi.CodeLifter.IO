using System;
using System.Threading.Tasks;
using CodeLifter.IO.Github.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace CodeLifter.IO.CovidApi.Functions.Controllers
{
    public static class AdminController
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

        [FunctionName("AdminController")]
        public static async Task Run([TimerTrigger("0 0 14-22 * * *")] TimerInfo myTimer, ILogger log)  //every two hours
        {
            await Service.DownloadAllFiles();
        }
    }
}