using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CovidApi.Settings;
using Microsoft.Extensions.Logging;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace CovidApi.Services
{
    public interface ISmsTextSender
    {
        void SendSMS(string smsText);
    }

    public class TwilioSmsService : ISmsTextSender
    {
        private readonly ILogger<TwilioSmsService> _logger;
        private readonly IEnvironmentService _envService;
        private readonly TwilioSettings _twilioSettings;

        public TwilioSmsService(ILogger<TwilioSmsService> logger,
                                IEnvironmentService envService,
                                TwilioSettings twilioSettings)
        {
            _logger = logger;
            _envService = envService;
            _twilioSettings = twilioSettings;
            if (_envService.IsDebug())
            {
                return;
            }

            try
            {
                TwilioClient.Init(_twilioSettings.AccountSID, _twilioSettings.AuthToken);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
            }
        }

        public void SendSMS(string smsText)
        {
            if (_envService.IsDebug())
            {
                return;
            }

            try
            {
                var message = MessageResource.Create(
                    body: smsText,
                    from: new Twilio.Types.PhoneNumber("+12057514753"),
                    to: new Twilio.Types.PhoneNumber("+13603331197")
                );
                _logger.LogInformation($"Message SID: {message.Sid}");
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
            }
        }
    }
}
