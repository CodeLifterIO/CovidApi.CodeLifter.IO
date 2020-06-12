using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

using Slugify;

namespace CodeLifter.Covid19.Data.Models
{
    public class Province : Entity
    {
        [JsonIgnore]
        public int? CountryId { get; set; }
        [JsonIgnore]
        public Country Country { get; set; }

        [JsonIgnore]
        public List<District> Districts { get; } = new List<District>();

        public string Name { get; set; }
        public string Slug { get; set; }

        [JsonIgnore]
        public int? GeoCoordinateId { get; set; }
        public GeoCoordinate GeoCoordinate {get; set;}

        [NotMapped]
        public string CountryUrl
        {
            get
            {
                return (null != Country?.Slug) ? $"country/{Country.Slug}" : null;
            }
        }
        //[NotMapped]
        //public string TotalsUrl
        //{
        //    get { return $"province/{Slug}"; }
        //}

        [NotMapped]
        public string DistrictsUrl
        {
            get { return $"province/{Slug}/districts"; }
        }

        [NotMapped]
        public string TimeSeriesUrl
        {
            get { return $"province/{Slug}/timeseries"; }
        }

        //[NotMapped]
        //public Statistic CurrentData { get; set; }

        [NotMapped]
        public List<Totals> TimeSeries { get; set; }


        public static Province Find(Province entity)
        {
            using (var context = new CovidContext())
            {
                // Province province = context.Provinces.Find(entity);
                // if(null != province) return province;

                return context.Provinces
                    .Where(p => p.Name == entity.Name || p.Slug == entity.Slug)
                    .FirstOrDefault();
            }
        }

        public static Province Upsert(Province entity)
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

            Province province = Find(entity);

            if(null == province)
            {
                Insert(entity);
                return entity;
            }
            else
            {
                Province.ShallowCopy(province, entity);
                Update(province);
            }

            return province;
        }
    }
}
