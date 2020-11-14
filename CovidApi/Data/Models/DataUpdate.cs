using CovidApi.Data;
using CovidApi.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

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
