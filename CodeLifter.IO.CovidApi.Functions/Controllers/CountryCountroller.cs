// using System;
// using System.IO;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Azure.WebJobs;
// using Microsoft.Azure.WebJobs.Extensions.Http;
// using Microsoft.AspNetCore.Http;
// using Microsoft.Extensions.Logging;
// using Newtonsoft.Json;
// using System.Collections.Generic;
// using CodeLifter.Covid19.Data.Models;
// using CodeLifter.Covid19.Data;
// using System.Linq;
// using Microsoft.EntityFrameworkCore;
// using System.Web.Http;

// namespace CodeLifter.IO.CovidApi.Functions.Controllers
// {
//     public static class CountryCountroller
//     {
//         [FunctionName("CountryProvinces")]
//         public static async Task<IActionResult> CountryProvinces(
//             [HttpTrigger(AuthorizationLevel.Function, "get", Route = "country/{slug}/provinces")]
//             HttpRequest req, ILogger log, string slug)
//         {
//             string searchTerm = req.Query["searchTerm"];

//             Country country = null;
//             using (var context = new CovidContext())
//             {
//                 country = await context.Countries
//                     .Where(c => c.Slug == slug)
//                     .Include(c => c.GeoCoordinate)
//                     .FirstOrDefaultAsync();

//                 var query = from dp in context.Set<DataPoint>()
//                             where dp.CountryId == country.Id
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
//             }

//             List<Province> provinces = null;
//             if (null != country)
//             {
//                 using (var context = new CovidContext())
//                 {
//                     var query = context.Provinces
//                         .Where(p => p.CountryId == country.Id);

//                     if (!string.IsNullOrWhiteSpace(searchTerm))
//                     {
//                         query = query.Where(p => p.Name.Contains(searchTerm) || p.Slug.Contains(searchTerm));
//                     }

//                     provinces = await query
//                         .Include(p => p.GeoCoordinate)
//                         .ToListAsync();
//                 }
//             }

//             return new OkObjectResult(provinces);
//         }


//         [FunctionName("Country")]
//         public static async Task<IActionResult> Country([HttpTrigger(AuthorizationLevel.Function, "get", Route = "country/{slug}")]
//         HttpRequest req, ILogger log, [FromRoute] string slug)
//         {
//             Country country = null;
//             using (var context = new CovidContext())
//             {
//                 country = await context.Countries
//                     .Where(c => c.Slug == slug)
//                     .Include(c => c.GeoCoordinate)
//                     .FirstOrDefaultAsync();

//                 var query = from dp in context.Set<DataPoint>()
//                             where dp.CountryId == country.Id
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
//                 country.TimeSeries = query.ToList();
//                 return new OkObjectResult(country);
//             }
//         }
//     }
// }
