using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeLifter.Covid19.Data.Models
{
    public class DataCollectionStatistic : Entity
    {
        public int RecordsProcessed { get; set; }
        public DateTime LastRunCompleted { get; set; }
        public DateTime LastRunStarted { get; set; }
        public string FileName { get; set; }
        [NotMapped]
        [JsonIgnore]
        public string ElapsedSeconds
        {
            get
            {
                return $"Elapsed(Seconds):{(LastRunCompleted - LastRunStarted).TotalSeconds}";
            }
        }

        public override string ToString()
        {
            return $"Records:{RecordsProcessed}  Filename:{FileName}  {ElapsedSeconds}";
        }
    }
}