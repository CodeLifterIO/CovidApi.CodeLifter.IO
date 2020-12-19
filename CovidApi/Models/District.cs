using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CovidApi.Data;
using CovidApi.Models.BaseEntities;
using Microsoft.EntityFrameworkCore;
using Slugify;

namespace CovidApi.Models
{
    public class District : SluggableEntity
    {
        public string FIPS { get; set; }

        [JsonIgnore]
        public int? GeoCoordinateId { get; set; }
        public GeoCoordinate GeoCoordinate { get; set; }

        public string ProvinceSlugId { get; set; }
        public string CountrySlugId { get; set; }

        [NotMapped]
        public Total CurrentData { get; set; }

        [NotMapped]
        public List<Total> TimeSeries { get; set; }
    }
}
