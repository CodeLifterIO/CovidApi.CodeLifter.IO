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
    public class Province : SluggableEntity
    {
        public string CountrySlugId { get; set; }
        public Country Country { get; set; }
        public int? GeoCoordinateId { get; set; }
        public GeoCoordinate GeoCoordinate { get; set; }

        [NotMapped]
        public Total CurrentData { get; set; }

        [NotMapped]
        public List<Total> TimeSeries { get; set; }
    }
}
