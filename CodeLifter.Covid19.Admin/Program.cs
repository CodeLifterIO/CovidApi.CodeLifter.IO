using CodeLifter.Covid19.Data;
using CodeLifter.Covid19.Data.Models;
using CodeLifter.IO.Github.Models;
using CodeLifter.IO.Github.Services;
using CodeLifter.Logging.Loggers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeLifter.Covid19.Admin
{
    class Program
    {
        public const string GithubFolderPath = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_daily_reports/";
        
        public static ILogger Logger => new ConsoleLogger();
        
        
        protected static GithubService _service { get; private set; }
        public static GithubService Service
        {
            get
            {
                if (_service == null)
                    _service = new GithubService(Environment.GetEnvironmentVariable("GITHUB_AUTH_TOKEN"));

                return _service;
            }
        }

        static void Main(string[] args)
        {
            string[] adminArgs = Environment.GetEnvironmentVariable("ADMIN_ARGS").Split(" ");
            if(adminArgs != null)
            {
                args = adminArgs;
            }
            
            Logger.LogEntry("******* STARTING UP ******", Logging.LogLevels.Info);

            if (args.Length == 1 && args[0] == "-a")
            {
                Task t = DownloadAllFiles();
                t.Wait();
            }
            else if(args.Length == 2 && args[0] == "-f")
            {
                Task t = DownloadFile(args[1]);
                t.Wait();
            }
            else
            {
                PrintArgs();
                return;
            }
        }

        public static void PrintArgs()
        {
            Logger.LogEntry("******* INVALID ARGS ******", Logging.LogLevels.Info);
            Logger.LogEntry("******* -a            Download all files ******", Logging.LogLevels.Info);
            Logger.LogEntry("******* -f [filename] Download single file ******", Logging.LogLevels.Info);
        }

        public static async Task DownloadFile(string fileName = null)
        {
            if (string.IsNullOrWhiteSpace(fileName) || !fileName.EndsWith(".csv"))
            {
                Logger.LogEntry("INVALID FILENAME", Logging.LogLevels.Error);
            }

            DataCollectionStatistic dcs = null;
            string downloadUrl = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_daily_reports/" + fileName;
            try
            {
                await Service.ParseAndDeleteFile(downloadUrl, fileName);
                await Service.SaveEntriesToDataModel(fileName);
            }
            catch
            {
                Logger.LogEntry($"FILE: {dcs.FileName} - DOES NOT EXIST OR IS IMPROPERLY FORMATTED", Logging.LogLevels.Info);
                Logger.LogEntry($"FAILED TO PROCESS FILE: {dcs.FileName}", Logging.LogLevels.Fatal);
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
            
            Logger.LogEntry("SUCCESS - UP TO DATE", Logging.LogLevels.Info);
        }
    }
}
