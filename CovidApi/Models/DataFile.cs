using CovidApi.Data;
using CovidApi.Models.BaseEntities;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace CovidApi.Models
{
    public class DataFile : BaseEntity
    {
        public int? RecordsProcessed { get; set; }
        public bool? Completed { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public byte[]? FileData { get; set; }
    }
}
