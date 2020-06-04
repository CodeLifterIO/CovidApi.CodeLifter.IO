using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using Slugify;

namespace CodeLifter.Covid19.Data.Models
{
    public class District : Entity
    {
        public string FIPS { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int? Confirmed { get; set; }
        public int? Deaths { get; set; }
        public int? Recovered { get; set; }
        public int? Active { get; set; }
        [JsonIgnore]
        public double? CaseFatalityRatio { get; set; }

        [JsonIgnore]
        public int? GeoCoordinateId { get; set; }
        public GeoCoordinate GeoCoordinate {get; set;}

        [JsonIgnore]
        public int? ProvinceId { get; set; }
        [JsonIgnore]
        public Province Province { get; set; }

        [JsonIgnore]
        public int? CountryId { get; set; }
        [JsonIgnore]
        public Country Country { get; set; }

        [NotMapped]
        public string CountryUrl
        {
            get {
                return (null != Country?.Slug) ? $"country/{Country.Slug}" : null;
            }
        }

        [NotMapped]
        public string ProvinceUrl
        {
            get
            {
                return (null != Province?.Slug) ? $"province/{Province.Slug}" : null;
            }
        }

        //[NotMapped]
        //public string TotalsUrl
        //{
        //    get { return $"district/{Slug}"; }
        //}

        [NotMapped]
        public string TimeSeriessUrl
        {
            get { return $"district/{Slug}/timeseries"; }
        }

        //[NotMapped]
        //public Statistic CurrentData { get; set; }

        [NotMapped]
        public List<Statistic> TimeSeries { get; set; }


        public static District Find(District entity)
        {
            using (var context = new CovidContext())
            {
                return context.Districts
                    .Where(p => p.Name == entity.Name || p.Slug == entity.Slug)
                    .FirstOrDefault();
            }
        }

        public static District Upsert(District entity)
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

            District district = Find(entity);

            if(null == district)
            {
                Insert(entity);
                return entity;
            }
            else
            {
                District.ShallowCopy(district, entity);
                Update(district);
            }

            return district;
        }
    }
}
