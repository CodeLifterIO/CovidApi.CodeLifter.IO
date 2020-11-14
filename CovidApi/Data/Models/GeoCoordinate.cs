using CovidApi.Data;
using CovidApi.Models.BaseEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CovidApi.Models  
{
    public class GeoCoordinate : BaseEntity
    {
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        
        //public static GeoCoordinate Find(double latitude, double longitude)
        //{
        //    using (var context = new CovidContext())
        //    {
        //        return Find(latitude, longitude, context);
        //    }
        //}

        //public static GeoCoordinate Find(double latitude, double longitude, CovidContext context)
        //{
        //    return context.GeoCoordinates
        //        .Where(geo => geo.Latitude == latitude && geo.Longitude == longitude)
        //        .FirstOrDefault();
        //}

        //public static GeoCoordinate Upsert(GeoCoordinate newGeo)
        //{
        //    using (var context = new CovidContext())
        //    {
        //        return Upsert(newGeo, context);
        //    }
        //}

        //public static GeoCoordinate Upsert(GeoCoordinate newGeo, CovidContext context)
        //{
        //    if (newGeo?.Latitude == null || newGeo?.Longitude == null)
        //        return null;

        //    int result = context.GeoCoordinates.Upsert(newGeo)
        //       .On(g => new { g.Latitude, g.Longitude } )
        //       .WhenMatched((eDB, eIn) => new GeoCoordinate
        //       {
        //           Latitude = newGeo.Latitude,
        //           Longitude = newGeo.Longitude,
        //           UpdatedAt = DateTime.UtcNow,
        //       })
        //       .Run();
        //    return Find((double)newGeo.Latitude, (double)newGeo.Longitude, context);
        //}
    }
}
