using CodeLifter.Covid19.Data.Models.BaseEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public string CombinedKey { get; set; }
        public string CountrySlugId { get; set; }
        public string ProvinceSlugId { get; set; }
        public string DistrictSlugId { get; set; }

        public static void Upsert(DataPoint newDp)
        {
            using (var context = new CovidContext())
            {
                Upsert(newDp, context);
            }
        }

        public static void Upsert(DataPoint newDp, CovidContext context)
        {
            int result = context.DataPoints.Upsert(newDp)
                .On(dp => new { dp.LastUpdate, dp.CountrySlugId, dp.ProvinceSlugId, dp.DistrictSlugId})
                .WhenMatched((eDB, eIn) => new DataPoint
                {
                    UpdatedAt = DateTime.UtcNow,
                    Confirmed = newDp.Confirmed,
                    Deaths = newDp.Deaths,
                    Recovered = newDp.Recovered,
                    Active = newDp.Active,
                    IncidenceRate = newDp.IncidenceRate,
                    CaseFatalityRatio = newDp.CaseFatalityRatio,
                    SourceFile = newDp.SourceFile,
                    CombinedKey = newDp.CombinedKey,
                    CountrySlugId = newDp.CountrySlugId,
                    ProvinceSlugId = newDp.ProvinceSlugId,
                    DistrictSlugId = newDp.DistrictSlugId
                })
                .Run();
        }

        public static void UpsertRange(List<DataPoint> newDps)
        {
            using (var context = new CovidContext())
            {
                UpsertRange(newDps, context);
            }
        }

        public static void UpsertRange(List<DataPoint> newDps, CovidContext context)
        {
            int result = context.DataPoints.UpsertRange(newDps)
                .On(dp => new { dp.LastUpdate, dp.CountrySlugId, dp.ProvinceSlugId, dp.DistrictSlugId })
                .WhenMatched((eDB, eIn) => new DataPoint
                {
                    UpdatedAt = DateTime.UtcNow,
                    Confirmed = eIn.Confirmed,
                    Deaths = eIn.Deaths,
                    Recovered = eIn.Recovered,
                    Active = eIn.Active,
                    IncidenceRate = eIn.IncidenceRate,
                    CaseFatalityRatio = eIn.CaseFatalityRatio,
                    SourceFile = eIn.SourceFile,
                    CombinedKey = eIn.CombinedKey,
                    CountrySlugId = eIn.CountrySlugId,
                    ProvinceSlugId = eIn.ProvinceSlugId,
                    DistrictSlugId = eIn.DistrictSlugId
                })
                .Run();
        }


        public static void AddRange(List<DataPoint> newDps)
        {
            using (var context = new CovidContext())
            {
                AddRange(newDps, context);
            }
        }

        public static void AddRange(List<DataPoint> newDps, CovidContext context)
        {
            context.DataPoints.AddRange(newDps);
            context.SaveChanges();
        }
    }
}
