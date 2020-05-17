using System;
namespace CovidApi.CodeLifter.IO.Management.Models
{
    public class ApiLimitReport
    {
        public int RequestsPerHour { get; set; }
        public int RemainingRequests { get; set; }
        public DateTimeOffset LimitResetTime { get; set; }
    }
}
