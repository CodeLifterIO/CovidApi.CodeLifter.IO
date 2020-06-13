// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Azure.WebJobs;
// using Microsoft.Azure.WebJobs.Extensions.Http;
// using Microsoft.AspNetCore.Http;
// using Microsoft.Extensions.Logging;
// using CodeLifter.Covid19.Data.Models;
// using CodeLifter.Covid19.Data;
// using Microsoft.EntityFrameworkCore;
// using System.Linq;

// namespace CodeLifter.IO.CovidApi.Functions.Controllers
// {
//     public static class DistrictController
//     {
//         [FunctionName("District")]
//         public static async Task<IActionResult> District(
//             [HttpTrigger(AuthorizationLevel.Function, "get", Route = "district/{slug}")]
//             HttpRequest req,
//             ILogger log,
//             string slug)
//         {
//             using (var context = new CovidContext())
//             {
//                 District district = await context.Districts
//                     .Where(d => d.Slug == slug)
//                     .Include(d => d.GeoCoordinate)
//                     .FirstOrDefaultAsync();

//                 var query = from dp in context.Set<DataPoint>()
//                             where dp.DistrictId == district.Id
//                             group dp by dp.SourceFile into s
//                             where s.Count() > 0
//                             orderby s.Key
//                             select new Statistic()
//                             {
//                                 SourceFile = s.Key,
//                                 Deaths = (int)s.Sum(x => x.Deaths),
//                                 Confirmed = (int)s.Sum(x => x.Deaths),
//                                 Recovered = (int)s.Sum(x => x.Deaths),
//                                 Active = (int)s.Sum(x => x.Active),
//                                 Count = s.Count()
//                             };
//                 district.TimeSeries = await query.ToListAsync();

//                 return new OkObjectResult(district);
//             }
//         }
//     }
// }
