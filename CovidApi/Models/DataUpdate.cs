using CovidApi.Models.BaseEntities;
using System;

namespace CovidApi.Models
{
    public class DataUpdate : BaseEntity
    {
        public int RecordsProcessed { get; set; } = 0;  
        public string StartFileName { get; set; }
        public string LastCompletedFileName { get; set; }
        public bool Completed { get; set; } = false;
        public DateTime CompletedAt { get; set; }
    }
}
