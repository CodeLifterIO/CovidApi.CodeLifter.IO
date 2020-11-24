using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CovidApi.Data;
using CovidApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CovidApi.Repositories
{
    public class GeoCoordinateRepository
    {
        private readonly CovidContext _context;

        public GeoCoordinateRepository(CovidContext context)
        {
            _context = context;
        }

        public GeoCoordinate Find(double latitude, double longitude)
        {
            return _context.GeoCoordinates
                .Where(geo => geo.Latitude == latitude && geo.Longitude == longitude)
                .FirstOrDefault();
        }

        public GeoCoordinate Upsert(GeoCoordinate geo)
        {
            if (geo?.Latitude == null || geo?.Longitude == null)
                return null;

            int result = _context.GeoCoordinates.Upsert(geo)
               .On(g => new { g.Latitude, g.Longitude })
               .WhenMatched((eDB, eIn) => new GeoCoordinate
               {
                   Latitude = geo.Latitude,
                   Longitude = geo.Longitude,
                   UpdatedAt = DateTime.UtcNow,
               })
               .Run();
            return geo;
        }
    }
}
