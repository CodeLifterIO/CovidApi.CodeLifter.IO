using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using CodeLifter.Covid19.Data.Models;
using System.Linq;
using CodeLifter.Covid19.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeLifter.IO.CovidApi.Functions.Controllers
{
    public static class GlobalController
    {
        [FunctionName("Global")]
        //[Route("[controller]")]
        public static async Task<IActionResult> Global(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "global")] HttpRequest req,
            ILogger log)
        {
            using (var context = new CovidContext())
            {
                Planet earth = new Planet();
                var tsQuery = from dp in context.Set<DataPoint>()
                              group dp by dp.SourceFile into s
                              where s.Count() > 0
                              orderby s.Key
                              select new Statistic()
                              {
                                  SourceFile = s.Key,
                                  Deaths = (int)s.Sum(x => x.Deaths),
                                  Confirmed = (int)s.Sum(x => x.Deaths),
                                  Recovered = (int)s.Sum(x => x.Deaths),
                                  Active = (int)s.Sum(x => x.Active),
                                  Count = s.Count()
                              };
                earth.TimeSeries = await tsQuery.ToListAsync();
                earth.CurrentData = earth.TimeSeries.Last();
                return new OkObjectResult(earth);
            }
        }

        [FunctionName("GlobalCountries")]
        //[HttpGet]
        //[Route("[controller]/[action]")]
        public static async Task<IActionResult> Countries(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "global/countries")] HttpRequest req, ILogger log)
        {
            using (var context = new CovidContext())
            {
                List<Country> countries = await context.Countries
                    .Include(country => country.GeoCoordinate)
                    .ToListAsync();
                return new OkObjectResult(countries);
            }
        }
    }
}
