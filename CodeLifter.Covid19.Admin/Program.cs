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
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace CodeLifter.Covid19.Admin
{
    class Program
    {
        const string accountSid = "ACf88381998d155e9618b0ec6566a77401";
        const string authToken = "ced5cbadbe221c19083219de1bfd6fa5";
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
            Environment.GetEnvironmentVariable("ADMIN_ARGS");

            TwilioClient.Init(accountSid, authToken);

            SendSMS($"******* CODELIFTER:API Starting *******");


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


        public static async Task DownloadAllFiles()
        {
            bool isStarted = false;
            isStarted = true;
            string lastFile = "";
            //using (var context = new CovidContext())
            //{
            //    List<DataCollectionStatistic> startFileStat = await context.DataCollectionStatistics.ToListAsync();
            //    lastFile = startFileStat?.Last()?.FileName;
            //}

            //if (string.IsNullOrWhiteSpace(startFile) && string.IsNullOrWhiteSpace(lastFile))
            //{
            //    isStarted = true;
            //}

            List<DataFile> files = new List<DataFile>();

            files = await Service.GetListOfFiles("CSSEGISandData",
                                                "COVID-19",
                                                "csse_covid_19_data/csse_covid_19_daily_reports");
            foreach (DataFile file in files)
            {
                if (isStarted == true)
                {
                    DateTime fileStart = DateTime.Now;

                    await Service.ParseAndDeleteFile(file);
                    await Service.SaveEntriesToDataModel(file.FileName);

                    DateTime fileComplete = DateTime.Now;
                    var elapsed = fileComplete - fileStart;

                    SendSMS($"File {file.FileName} completed in {elapsed.Seconds}");
                }

                if (file.FileName == lastFile)
                {
                    isStarted = true;
                }
            }

            SendSMS("SUCCESS - UP TO DATE");
        }


        public static void SendSMS(string smsText)
        {
            var message = MessageResource.Create(
                body: smsText,
                from: new Twilio.Types.PhoneNumber("+12057514753"),
                to: new Twilio.Types.PhoneNumber("+13603331197")
            );
            Console.WriteLine(message.Sid);

            Logger.LogEntry(smsText, Logging.LogLevels.Info);
        }
    }
}
