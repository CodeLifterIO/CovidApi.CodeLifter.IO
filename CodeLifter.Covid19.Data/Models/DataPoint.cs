using System;
using System.Linq;

namespace CodeLifter.Covid19.Data.Models
{
    public class DataPoint : Entity
    {
        public DateTime LastUpdate { get; set; }
        public int? Confirmed { get; set; }
        public int? Deaths { get; set; }
        public int? Recovered { get; set; }
        public int? Active { get; set; }
        public double? IncidenceRate { get; set; }
        public double? CaseFatalityRatio { get; set; }
        public string SourceFile { get; set; }

        public int? CountryId { get; set; }
        public Country Country { get; set; }

        public int? ProvinceId { get; set; }
        public Province Province { get; set; }

        public int? DistrictId { get; set; }
        public District District { get; set; }



        public static DataPoint Find(DataPoint entity)
        {
            if(0 != entity.Id)
                return entity;

            using (var context = new CovidContext())
            {
                return context.DataPoints
                    .Where(dp => dp.LastUpdate == entity.LastUpdate 
                                && dp.CountryId == entity.CountryId
                                && dp.ProvinceId == entity.ProvinceId
                                && dp.DistrictId == entity.DistrictId)
                    .FirstOrDefault();
            }
        }


        public static DataPoint Upsert(DataPoint entity)
        {
            if(null == entity)
            {
                return null;
            }

            DataPoint dataPt = Find(entity);

            if(null == dataPt)
            {
                Insert(entity);
            }
            else
            {
                DataPoint.ShallowCopy(dataPt, entity);
                Update(dataPt);
            }

            return dataPt;
        }
    }
}
