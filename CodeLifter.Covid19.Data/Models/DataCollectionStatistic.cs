using System;

namespace CodeLifter.Covid19.Data.Models
{
    public class DataCollectionStatistic : Entity
    {
        public int RecordsProcessed { get; set; }
        public DateTime LastRunCompleted { get; set; }
        public DateTime LastRunStarted { get; set; }
        public string FileName { get; set; }

        public override string ToString()
        {
            return $"Records:{RecordsProcessed}  Filename:{FileName}  Elapsed(ms):{(LastRunCompleted - LastRunStarted).TotalMilliseconds}";
        }
    }
}