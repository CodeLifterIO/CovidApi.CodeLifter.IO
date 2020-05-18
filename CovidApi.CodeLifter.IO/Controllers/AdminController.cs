﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using CodeLifter.Covid19.Data.Models;
using CovidApi.CodeLifter.IO.Management.Models;
using CovidApi.CodeLifter.IO.Management.Services;
using CodeLifter.Covid19.Data;

namespace CovidApi.CodeLifter.IO.Controllers
{
    public class AdminController
    {
        public const string GithubFolderPath = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_daily_reports/";

        protected GithubService _service { get; private set; }
        public GithubService Service
        {
            get
            {
                if (_service == null)
                    _service = new GithubService(Environment.GetEnvironmentVariable("GITHUB_AUTH_TOKEN"));

                return _service;
            }
        }


        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
        }




        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Info()
        {
            ApiLimitReport report = null;
            List<DataFile> files = new List<DataFile>();
            DataCollectionStatistic dcs;
            try
            {
                report = await Service.ReportAPILimits();
                files = await Service.GetListOfFiles("CSSEGISandData",
                                                    "COVID-19",
                                                    "csse_covid_19_data/csse_covid_19_daily_reports");
            }
            catch
            {
                return new BadRequestResult();
            }

            var returnStruct = new { ApiLimitReport = report, DataFiles = files };
            return new OkObjectResult(returnStruct);
        }


        [HttpGet]
        [Route("[controller]/[action]/{fileName}")]
        public async Task<IActionResult> ProcessFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName) || !fileName.EndsWith(".csv"))
            {
                return new BadRequestResult();
            }

            DataCollectionStatistic dcs = null;
            string downloadUrl = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_daily_reports/" + fileName;
            try
            {
                await Service.ParseAndDeleteFile(downloadUrl, fileName);
                dcs = await Service.SaveEntriesToDataModel(fileName);
            }
            catch
            {
                return new BadRequestResult();
            }

            return new OkObjectResult(dcs);
        }

        //[FunctionName("TimerTrigger_Update_Database")]
        //public async Task RunAsync([TimerTrigger("0 */48 * * * *")] TimerInfo myTimer, ILogger log)
        //{


        //    await DownloadAllFiles(null);
        //}

        [HttpGet]
        [Route("[controller]/[action]")]
        [Route("[controller]/[action]/{fileName}")]
        public async Task<IActionResult> Process_All(string fileName)
        {
            return new OkObjectResult(await DownloadAllFiles(fileName));
        }


        public async Task<DataCollectionStatistic> DownloadAllFiles(string startFile = null)
        {
            bool isStarted = false;

            if (string.IsNullOrWhiteSpace(startFile))
            {
                isStarted = true;
            }

            DataCollectionStatistic dcs = null;
            List<DataFile> files = new List<DataFile>();

            files = await Service.GetListOfFiles("CSSEGISandData",
                                                "COVID-19",
                                                "csse_covid_19_data/csse_covid_19_daily_reports");
            foreach (DataFile file in files)
            {
                if (isStarted == true)
                {
                    await Service.ParseAndDeleteFile(file);
                    dcs = await Service.SaveEntriesToDataModel(file.FileName);
                }

                if (file.FileName == startFile)
                {
                    isStarted = true;
                }
            }

            return dcs;
        }

    }
}