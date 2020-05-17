//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Threading.Tasks;
//using CodeLifter.Covid19.Data;
//using CodeLifter.Covid19.Data.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Azure.WebJobs;
//using Microsoft.Azure.WebJobs.Extensions.Http;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;

//namespace CovidApi.CodeLifter.IO.Controllers
//{
//    public class DistrictsController : BaseController
//    {
//        [FunctionName("HttpTrigger_Districts_GetAll")]
//        public async Task<HttpResponseMessage> Districts(
//            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "districts")] HttpRequest req,
//            ILogger log)
//        {
//            List<District> districts;
//            using (var context = new CovidContext())
//            {
//                districts = await context.Districts
//                    .Include(p => p.GeoCoordinate)
//                    .Include(p => p.Country)
//                    .Include(p => p.Province)
//                    .ToListAsync();
//            }

//            return ConvertToJsonAndReturnOK(districts);
//        }


//        [FunctionName("HttpTrigger_District_Get")]
//        public async Task<HttpResponseMessage> District(
//            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "district/{slug}")] HttpRequest req,
//            string slug,
//            ILogger log)
//        {
//            District district;
//            using (var context = new CovidContext())
//            {
//                district = await context.Districts
//                    .Where(d => d.Slug == slug)
//                    .Include(d => d.GeoCoordinate)
//                    .Include(d => d.Province)
//                    .Include(d => d.Country)
//                    .FirstOrDefaultAsync();

//                var query = from dp in context.Set<DataPoint>()
//                            where dp.DistrictId == district.Id
//                            group dp by dp.SourceFile into s
//                            where s.Count() > 0
//                            orderby s.Key
//                            select new Statistic()
//                            {
//                                SourceFile = s.Key,
//                                Deaths = (int)s.Sum(x => x.Deaths),
//                                Confirmed = (int)s.Sum(x => x.Deaths),
//                                Recovered = (int)s.Sum(x => x.Deaths),
//                                Active = (int)s.Sum(x => x.Active),
//                                Count = s.Count()
//                            };
//                district.CurrentData = query.Last();
//                return ConvertToJsonAndReturnOK(district);
//            }
//        }

//        [FunctionName("HttpTrigger_District_GetTimeSeries")]
//        public async Task<HttpResponseMessage> GetTimeSeriesByDistrict(
//            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "district/{slug}/timeseries")] HttpRequest req,
//            string slug,
//            ILogger log)
//        {
//            using (var context = new CovidContext())
//            {
//                District district = await context.Districts
//                    .Where(d => d.Slug == d.Slug)
//                    .Include(d => d.GeoCoordinate)
//                    .FirstOrDefaultAsync();

//                var query = from dp in context.Set<DataPoint>()
//                            where dp.DistrictId == district.Id
//                            group dp by dp.SourceFile into s
//                            where s.Count() > 0
//                            orderby s.Key
//                            select new Statistic()
//                            {
//                                SourceFile = s.Key,
//                                Deaths = (int)s.Sum(x => x.Deaths),
//                                Confirmed = (int)s.Sum(x => x.Deaths),
//                                Recovered = (int)s.Sum(x => x.Deaths),
//                                Active = (int)s.Sum(x => x.Active),
//                                Count = s.Count()
//                            };
//                district.TimeSeries = query;

//                return ConvertToJsonAndReturnOK(district);
//            }
//        }
//    }
//}
