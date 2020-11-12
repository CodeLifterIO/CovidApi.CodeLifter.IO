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
        public District() { }
        public District(string name, string fips = null)
        {
            Name = name;
            SlugHelper slugger = new SlugHelper();
            SlugId = slugger.GenerateSlug(Name);

            if (!string.IsNullOrWhiteSpace(fips)) FIPS = fips;
        }

        public string FIPS { get; set; }

        [JsonIgnore]
        public int? GeoCoordinateId { get; set; }
        public GeoCoordinate GeoCoordinate { get; set; }

        public string ProvinceSlugId { get; set; }
        //public Province Province { get; set; }
        public string CountrySlugId { get; set; }
        //public Country Country { get; set; }

        [NotMapped]
        public Total CurrentData { get; set; }

        [NotMapped]
        public List<Total> TimeSeries { get; set; }

        //public static District Find(string slug)
        //{
        //    using (var context = new CovidContext())
        //    {
        //        return context.Districts
        //            .Where(c => c.SlugId == slug)
        //            .FirstOrDefault();
        //    }
        //}

        //public static District Find(string slug, CovidContext context)
        //{
        //    return context.Districts
        //        .Where(c => c.SlugId == slug)
        //        .FirstOrDefault();
        //}

        //public static District Upsert(District newDistrict)
        //{
        //    using (var context = new CovidContext())
        //    {
        //        return Upsert(newDistrict, context);
        //    }
        //}

        //public static District Upsert(District newDistrict, CovidContext context)
        //{
        //    int result = context.Districts.Upsert(newDistrict)
        //       .On(e => e.SlugId)
        //       .WhenMatched((eDB, eIn) => new District
        //       {
        //           Name = newDistrict.Name,
        //           UpdatedAt = DateTime.UtcNow,
        //           GeoCoordinateId = newDistrict.GeoCoordinateId,
        //       })
        //       .Run();
        //    return Find(newDistrict.SlugId, context);
        //}
    }
}
