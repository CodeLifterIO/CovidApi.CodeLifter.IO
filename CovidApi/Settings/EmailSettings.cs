using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidApi.Settings
{
    public class EmailSettings
    {
        public string AdminEmail { get; set; }
        public bool IsDevelopment { get; set; }
        public int MailPort { get; set; }
        public string MailServer { get; set; }
        public string Password { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string SmtpUserName { get; set; }
        public string SesSmtpUser { get; set; }
        public bool UseSsl { get; set; }
    }
}
