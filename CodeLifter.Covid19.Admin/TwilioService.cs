using System;
using CodeLifter.Logging.Loggers;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace CodeLifter.Covid19.Admin
{
    public class TwilioService
    {
        public static ILogger Logger { get; set; }

        private string TwilioAccountSID = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
        private string TwilioAuthToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");

        public bool IsInittialized { get; set; }

        public TwilioService(ILogger log)
        {
            Logger = log;
            try
            {
                TwilioClient.Init(TwilioAccountSID, TwilioAuthToken);
                IsInittialized = true;
            }
            catch
            {
                IsInittialized = false;
            }
            
        }

        public void SendSMS(string smsText)
        {
            if(IsInittialized == false)
            {
                Logger.LogEntry("<---------- Twilio not Initialized -------->");
                return;
            }

            var message = MessageResource.Create(
                body: smsText,
                from: new Twilio.Types.PhoneNumber("+12057514753"),
                to: new Twilio.Types.PhoneNumber("+13603331197")
            );

            Logger.LogEntry($"Message SID: {message.Sid}");
            Logger.LogEntry(smsText, Logging.LogLevels.Info);
        }
    }
}
