using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CovidApi.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CovidApi.Controllers
{
    [ApiController]
    public class SettingsController : Controller
    {
        private readonly GithubSettings _githubSettings;
        private readonly EmailSettings _emailSettings;
        private readonly ConnectionStrings _connectionStrings;
        private readonly LoggingSettings _loggingSettings;

        public SettingsController(IOptionsMonitor<GithubSettings> ghOptionsMonitor,
                                  IOptionsMonitor<EmailSettings> emailOptionsMonitor,
                                  IOptionsMonitor<ConnectionStrings> _connectionStringsMonitor,
                                  IOptionsMonitor<LoggingSettings> loggingMonitor)
        {
            _githubSettings = ghOptionsMonitor.CurrentValue;
            _emailSettings = emailOptionsMonitor.CurrentValue;
            _connectionStrings = _connectionStringsMonitor.CurrentValue;
            _loggingSettings = loggingMonitor.CurrentValue;
        }

        [HttpGet("[controller]/[action]")]
        public IActionResult Index()
        {
            List<Object> settings = new List<object>();
            settings.Add(_githubSettings);
            settings.Add(_emailSettings);
            settings.Add(_connectionStrings);
            settings.Add(_loggingSettings);
            return Ok(settings);
        }
    }
}
