using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CovidApi.Data;
using CovidApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CovidApi.Repositories
{
    public interface IDistrictRepository
    {
        Task<District> FindAsync(string slug);
        Task UpsertAsync(District district);
        Task UpsertRangeAsync(List<District> districts);
    }

    public class DistrictRepository : IDistrictRepository
    {
        private readonly CovidContext _context;

        public DistrictRepository(CovidContext context)
        {
            _context = context;
        }

        public async Task<District> FindAsync(string slug)
        {
            return await _context.Districts
                .Where(d => d.SlugId == slug)
                .FirstOrDefaultAsync();
        }

        public async Task UpsertAsync(District district)
        {
            await _context.Districts.Upsert(district)
               .On(d => d.SlugId)
               .WhenMatched((eDB, eIn) => new District
               {
                   Name = district.Name,
                   UpdatedAt = DateTime.UtcNow,
                   GeoCoordinateId = district.GeoCoordinateId,
               })
               .RunAsync();
        }

        public async Task UpsertRangeAsync(List<District> districts)
        {
            await _context.Districts.UpsertRange(districts)
               .On(d => d.SlugId)
               .WhenMatched((eDB, eIn) => new District
               {
                   Name = eIn.Name,
                   UpdatedAt = DateTime.UtcNow,
                   GeoCoordinateId = eIn.GeoCoordinateId,
               })
               .RunAsync();
        }
    }
}
