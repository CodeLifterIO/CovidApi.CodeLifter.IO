using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CodeLifter.Covid19.Data;
using CodeLifter.Covid19.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CovidApi.CodeLifter.IO.Controllers
{
    public class GlobalController : BaseController
    {

        [HttpGet]
        [Route("[controller]/[Action]")]
        public async Task<IActionResult> Countries()
        {
            using (var context = new CovidContext())
            {
                List<Country> countries = await context.Countries
                    .Include(country => country.GeoCoordinate)
                    .ToListAsync();
                return new OkObjectResult(countries);
            }
        }

        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> Global()
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
    }
}
