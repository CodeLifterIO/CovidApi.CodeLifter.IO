using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidApi.Settings
{
    public class ConnectionStrings
    {
        public string DatabaseConnection { get; set; }
        public string RedisConnection { get; set; }
    }
}
