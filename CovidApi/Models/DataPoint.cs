using CovidApi.Data;
using CovidApi.Models.BaseEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidApi.Models
{
    public class DataPoint : BaseEntity
    {
        public DateTime LastUpdate { get; set; }
        public int? Confirmed { get; set; }
        public int? Deaths { get; set; }
        public int? Recovered { get; set; }
        public int? Active { get; set; }
        public double? IncidenceRate { get; set; }
        public double? CaseFatalityRatio { get; set; }
        public string SourceFile { get; set; }
        public string CombinedKey { get; set; }
        public string CountrySlugId { get; set; }
        public string ProvinceSlugId { get; set; }
        public string DistrictSlugId { get; set; }
    }
}
