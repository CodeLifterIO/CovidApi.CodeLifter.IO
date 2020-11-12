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
        public Province() { }
        public Province(string name)
        {
            Name = name;
            SlugHelper slugger = new SlugHelper();
            SlugId = slugger.GenerateSlug(Name);
        }

        public string CountrySlugId { get; set; }
        public Country Country { get; set; }
        public int? GeoCoordinateId { get; set; }
        public GeoCoordinate GeoCoordinate { get; set; }

        [NotMapped]
        public Total CurrentData { get; set; }

        [NotMapped]
        public List<Total> TimeSeries { get; set; }

        //public static Province Find(string slug)
        //{
        //    using (var context = new CovidContext())
        //    {
        //        return Find(slug, context);
        //    }
        //}

        //public static Province Find(string slug, CovidContext context)
        //{
        //    return context.Provinces
        //        .Where(c => c.SlugId == slug)
        //        .FirstOrDefault();
        //}

        //public static Province Upsert(Province newProvince)
        //{
        //    using (var context = new CovidContext())
        //    {
        //        return Upsert(newProvince, context);
        //    }
        //}

        //public static Province Upsert(Province newProvince, CovidContext context)
        //{
        //    int result = context.Provinces.Upsert(newProvince)
        //       .On(e => e.SlugId)
        //       .WhenMatched((eDB, eIn) => new Province
        //       {
        //           Name = newProvince.Name,
        //           UpdatedAt = DateTime.UtcNow,
        //           GeoCoordinateId = newProvince.GeoCoordinateId,
        //           CountrySlugId = newProvince.CountrySlugId
        //       })
        //       .Run();
        //    return Find(newProvince.SlugId);
        //}
    }
}
