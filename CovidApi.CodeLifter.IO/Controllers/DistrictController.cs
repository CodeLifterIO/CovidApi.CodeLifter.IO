using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CodeLifter.Covid19.Data;
using CodeLifter.Covid19.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CovidApi.CodeLifter.IO.Controllers
{
    public class DistrictController : BaseController
    {
        //[HttpGet]
        //[Route("[controller]/{slug}")]
        //public async Task<IActionResult> Data(string slug)
        //{
        //    using (var context = new CovidContext())
        //    {
        //        District district = await context.Districts
        //            .Where(d => d.Slug == slug)
        //            .Include(d => d.GeoCoordinate)
        //            .FirstOrDefaultAsync();

        //        var query = from dp in context.Set<DataPoint>()
        //                    where dp.DistrictId == district.Id
        //                    group dp by dp.SourceFile into s
        //                    where s.Count() > 0
        //                    orderby s.Key
        //                    select new Statistic()
        //                    {
        //                        SourceFile = s.Key,
        //                        Deaths = (int)s.Sum(x => x.Deaths),
        //                        Confirmed = (int)s.Sum(x => x.Deaths),
        //                        Recovered = (int)s.Sum(x => x.Deaths),
        //                        Active = (int)s.Sum(x => x.Active),
        //                        Count = s.Count()
        //                    };
        //        district.TimeSeries = await query.ToListAsync();

        //        return new OkObjectResult(district);
        //    }
        //}
    }
}
