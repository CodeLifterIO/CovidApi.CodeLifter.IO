using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidApi.Settings
{
    public class DatabaseSettings
    {
        public string DataSource { get; set; }
        public string Catalog { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
    }
}
