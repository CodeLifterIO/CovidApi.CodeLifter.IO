using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CovidApi.Data;
using CovidApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CovidApi.Repositories
{
    public interface IDataPointRepository
    {
        Task AddRangeAsync(List<DataPoint> newDps);
        Task UpsertAsync(DataPoint newDp);
        Task UpsertRangeAsync(List<DataPoint> newDps);
    }

    public class DataPointRepository : IDataPointRepository
    {
        private readonly CovidContext _context;

        public DataPointRepository(CovidContext context)
        {
            _context = context;
        }

        public async Task UpsertAsync(DataPoint newDp)
        {
            await _context.DataPoints.Upsert(newDp)
                 .On(dp => new { dp.LastUpdate, dp.CountrySlugId, dp.ProvinceSlugId, dp.DistrictSlugId })
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
                 .RunAsync();
        }

        public async Task UpsertRangeAsync(List<DataPoint> newDps)
        {
            await _context.DataPoints.UpsertRange(newDps)
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
                .RunAsync();
        }

        public async Task AddRangeAsync(List<DataPoint> newDps)
        {
            await _context.DataPoints.AddRangeAsync(newDps);
            await _context.SaveChangesAsync();
        }
    }
}
