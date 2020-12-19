using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CovidApi.Data;
using CovidApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CovidApi.Repositories
{
    public interface IGeoCoordinateRepository
    {
        Task<GeoCoordinate> FindAsync(double latitude, double longitude);
        Task<GeoCoordinate> UpsertAsync(GeoCoordinate geo);
    }

    public class GeoCoordinateRepository : IGeoCoordinateRepository
    {
        private readonly CovidContext _context;

        public GeoCoordinateRepository(CovidContext context)
        {
            _context = context;
        }

        public async Task<GeoCoordinate> FindAsync(double latitude, double longitude)
        {
            return await _context.GeoCoordinates
                .Where(geo => geo.Latitude == latitude && geo.Longitude == longitude)
                .FirstOrDefaultAsync();
        }

        public async Task<GeoCoordinate> UpsertAsync(GeoCoordinate geo)
        {
            if (geo?.Latitude == null || geo?.Longitude == null)
                return geo;

            await _context.GeoCoordinates.Upsert(geo)
               .On(g => new { g.Latitude, g.Longitude })
               .WhenMatched((eDB, eIn) => new GeoCoordinate
               {
                   UpdatedAt = DateTime.UtcNow,
               })
               .RunAsync();
            return geo;
        }
    }
}
