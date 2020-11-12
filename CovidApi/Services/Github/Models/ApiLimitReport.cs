using System;
namespace CovidApi.Models
{
    public class ApiLimitReport
    {
        public int RequestsPerHour { get; set; }
        public int RemainingRequests { get; set; }
        public DateTimeOffset LimitResetTime { get; set; }
    }
}
