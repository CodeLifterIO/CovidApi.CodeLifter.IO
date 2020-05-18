using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CodeLifter.Covid19.Data;
using CodeLifter.Covid19.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace CovidApi.CodeLifter.IO.Controllers
{
    public class GlobalController : BaseController
    {
        [HttpGet]
        [Route("global/timeseries")]
        public async Task<IActionResult> TimeSeries()
        {
            using (var context = new CovidContext())
            {
                Planet earth = new Planet();
                var query = from dp in context.Set<DataPoint>()
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
                earth.TimeSeries = query.ToList();
                return new OkObjectResult(earth);
            }
        }
    }
}
