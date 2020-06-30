using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CodeLifter.Covid19.Data.Models.BaseEntities;
using Microsoft.EntityFrameworkCore;
using Slugify;

namespace CodeLifter.Covid19.Data.Models
{
    public class Country : SluggableEntity
    {
        public Country() { }
        public Country(string name)
        {
            Name = name;
            SlugHelper slugger = new SlugHelper();
            SlugId = slugger.GenerateSlug(Name);
        }

        //[JsonIgnore]
        //public List<Province> Provinces { get; } = new List<Province>();

        public int? GeoCoordinateId { get; set; }
        public GeoCoordinate GeoCoordinate { get; set; }

        //[NotMapped]
        //public List<Total> TimeSeries { get; set; }

        public static Country Find(string slug)
        {
            using (var context = new CovidContext())
            {
                return Find(slug, context);
            }
        }

        public static Country Find(string slug, CovidContext context)
        {
            return context.Countries
                .Where(c => c.SlugId == slug)
                .FirstOrDefault();
        }

        public static Country Upsert(Country newCountry)
        {
            using (var context = new CovidContext())
            {
                return Upsert(newCountry, context);
            }
        }

        public static Country Upsert(Country newCountry, CovidContext context)
        {
            int result = context.Countries.Upsert(newCountry)
                .On(e => e.SlugId)
                .WhenMatched((eDB, eIn) => new Country
                {
                    Name = newCountry.Name,
                    UpdatedAt = DateTime.UtcNow,
                    GeoCoordinateId = newCountry.GeoCoordinateId,
                })
                .Run();
            return Find(newCountry.SlugId, context);
        }
    }
}
