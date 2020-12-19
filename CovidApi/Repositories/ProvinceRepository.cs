using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CovidApi.Data;
using CovidApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CovidApi.Repositories
{
    public interface IProvinceRepository
    {
        Task<Province> FindAsync(string slug);
        Task UpsertAsync(Province province);
        Task UpsertRangeAsync(List<Province> provinces);
    }

    public class ProvinceRepository : IProvinceRepository
    {
        private readonly CovidContext _context;

        public ProvinceRepository(CovidContext context)
        {
            _context = context;
        }

        public async Task<Province> FindAsync(string slug)
        {
            return await _context.Provinces
                .Where(d => d.SlugId == slug)
                .FirstOrDefaultAsync();
        }

        public async Task UpsertAsync(Province province)
        {
            await _context.Provinces.Upsert(province)
               .On(p => p.SlugId)
               .WhenMatched((eDB, eIn) => new Province
               {
                   Name = province.Name,
                   UpdatedAt = DateTime.UtcNow,
                   GeoCoordinateId = province.GeoCoordinateId,
               })
               .RunAsync();
        }


        public async Task UpsertRangeAsync(List<Province> provinces)
        {
            await _context.Provinces.UpsertRange(provinces)
               .On(p => p.SlugId)
               .WhenMatched((eDB, eIn) => new Province
               {
                   Name = eIn.Name,
                   UpdatedAt = DateTime.UtcNow,
                   GeoCoordinateId = eIn.GeoCoordinateId,
               })
               .RunAsync();
        }

    }
}
