using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using CodeLifter.Covid19.Data.Interfaces;
using Slugify;

namespace CodeLifter.Covid19.Data.Models
{
    public class Country : Entity, INamedEntity, IStatisticalEntity
    {
        [JsonIgnore]
        public List<Province> Provinces { get; } = new List<Province>();

        public string Name { get; set; }

        public string Slug { get; set; }

        [JsonIgnore]
        public int? GeoCoordinateId { get; set; }

        public GeoCoordinate GeoCoordinate {get; set;}

        [JsonIgnore]
        public List<DataPoint> DataPoints { get; set; }

        [NotMapped]
        public string TotalsUrl { get { return $"country/{Slug}"; } }

        [NotMapped]
        public string ProvincesUrl { get { return $"country/{Slug}/provinces"; } }

        [NotMapped]
        public string TimeSeriesUrl { get { return $"country/{Slug}/timeseries"; } }

        [NotMapped]
        public Statistic CurrentData { get; set; }

        [NotMapped]
        public List<Statistic> TimeSeries { get; set; }


        public static Country Find(Country entity)
        {
            using (var context = new CovidContext())
            {
                return context.Countries
                    .Where(c => c.Name == entity.Name || c.Slug == entity.Slug)
                    .FirstOrDefault();
            }
        }

        public static Country Upsert(Country entity)
        {
            if(null == entity)
            {
                return null;
            }
            else if(string.IsNullOrWhiteSpace(entity.Name))
            {
                return null;
            }
            else if(string.IsNullOrWhiteSpace(entity.Slug))
            {
                SlugHelper slugger = new SlugHelper();
                entity.Slug = slugger.GenerateSlug(entity.Name);
            }

            Country country = Find(entity);

            if(null == country)
            {
                Insert(entity);
                return entity;
            }
            else
            {
                Country.ShallowCopy(country, entity);
                Update(country);
            }

            return country;
        }
    }
}
