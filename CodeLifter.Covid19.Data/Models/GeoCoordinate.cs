using System.Linq;

namespace CodeLifter.Covid19.Data.Models  
{
    public class GeoCoordinate : Entity
    {
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public static GeoCoordinate Find(GeoCoordinate entity)
        {
            if(0 != entity.Id)
                return entity;

            using (var context = new CovidContext())
            {
                return context.GeoCoordinates
                    .Where(g => g.Latitude == entity.Latitude && g.Longitude == entity.Longitude)
                    .FirstOrDefault();
            }
        }

        public static GeoCoordinate Upsert(GeoCoordinate entity)
        {
            if(null == entity)
            {
                return null;
            }
            else if(null == entity.Latitude || null == entity.Longitude)
            {
                return null;
            }

            GeoCoordinate geo = Find(entity);

            if(null == geo)
            {
                Insert(entity);
            }
            else
            {
                GeoCoordinate.ShallowCopy(geo, entity);
                Update(geo);
            }

            return geo;
        }
    }
}