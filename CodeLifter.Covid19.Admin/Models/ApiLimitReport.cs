using System;
namespace CodeLifter.Covid19.Admin.Models
{
    public class ApiLimitReport
    {
        public int RequestsPerHour { get; set; }
        public int RemainingRequests { get; set; }
        public DateTimeOffset LimitResetTime { get; set; }
    }
}
