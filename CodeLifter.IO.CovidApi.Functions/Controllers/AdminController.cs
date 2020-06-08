using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeLifter.Covid19.Data;
using CodeLifter.Covid19.Data.Models;
using CodeLifter.IO.Github.Models;
using CodeLifter.IO.Github.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;

namespace CodeLifter.IO.CovidApi.Functions.Controllers
{
    public static class AdminController
    {
        public const string GithubFolderPath = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_daily_reports/";
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
        public static async void Run([TimerTrigger("0 0 */2 * * *")] TimerInfo myTimer, ILogger log)  //every two hours
        //public static async void Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log)  //every minute
        {
            DateTime startTime = DateTime.Now;
            log.LogInformation($"******* STARTING UP at {startTime} ******");

            await DownloadAllFiles();

            DateTime finishTime = DateTime.Now;
            log.LogInformation($"******* FINISHED at {finishTime} ******");
            log.LogInformation($"******* TIME ELAPSED {finishTime - startTime} ******");
        }

        public static void SendText()
        {
            //Twilio.HttpClient.SendMessage(
            //    "+614xxxxxxxx", // Insert your Twilio from SMS number here
            //    "+614xxxxxxxx", // Insert your verified (trial) to SMS number here
            //    "hello from Azure Functions!" + DateTime.Now
//);
        }

        public static async Task DownloadFile(string fileName = null)
        {
            DataCollectionStatistic dcs = null;
            string downloadUrl = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_daily_reports/" + fileName;
            try
            {
                await Service.ParseAndDeleteFile(downloadUrl, fileName);
                await Service.SaveEntriesToDataModel(fileName);
            }
            catch
            {
                Console.WriteLine($"FILE: {dcs.FileName} - DOES NOT EXIST OR IS IMPROPERLY FORMATTED");
                Console.WriteLine($"FAILED TO PROCESS FILE: {dcs.FileName}");
            }
        }


        public static async Task DownloadAllFiles(string startFile = null)
        {
            bool isStarted = false;

            string lastFile = "";
            using (var context = new CovidContext())
            {
                List<DataCollectionStatistic> startFileStat = await context.DataCollectionStatistics.ToListAsync();
                lastFile = startFileStat?.Last()?.FileName;
            }

            if (string.IsNullOrWhiteSpace(startFile) && string.IsNullOrWhiteSpace(lastFile))
            {
                isStarted = true;
            }

            List<DataFile> files = new List<DataFile>();

            files = await Service.GetListOfFiles("CSSEGISandData",
                                                "COVID-19",
                                                "csse_covid_19_data/csse_covid_19_daily_reports");
            foreach (DataFile file in files)
            {
                if (file.FileName == startFile)
                {
                    isStarted = true;
                }

                if (isStarted == true)
                {
                    await Service.ParseAndDeleteFile(file);
                    await Service.SaveEntriesToDataModel(file.FileName);
                }

                if (file.FileName == lastFile)
                {
                    isStarted = true;
                }
            }
        }
    }
}