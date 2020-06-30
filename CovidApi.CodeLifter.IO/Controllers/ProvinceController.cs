using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CodeLifter.Covid19.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using CodeLifter.Covid19.Data.Models;

namespace CovidApi.CodeLifter.IO.Controllers
{
    public class ProvinceController : BaseController
    {
        [HttpGet]
        [Route("[controller]/{slug}/[action]")]
        public async Task<IActionResult> Districts([FromRoute] string slug, [FromQuery]string searchTerm = "")
        {
            Province province;
            using (var context = new CovidContext())
            {
                province = await context.Provinces
                    .Where(p => p.SlugId == slug)
                    .Include(p => p.GeoCoordinate)
                    .Include(p => p.Country)
                    .FirstOrDefaultAsync();
            }

            List<District> districts = null;
            if (null != province)
            {
                using (var context = new CovidContext())
                {
                    var query = context.Districts
                        .Where(p => p.ProvinceSlugId == province.SlugId);

                    if (!string.IsNullOrWhiteSpace(searchTerm))
                    {
                        query = query.Where(d => d.Name.Contains(searchTerm) || d.Slug.Contains(searchTerm));
                    }

                    districts = await query.Include(p => p.Country)
                        .Include(p => p.Province)
                        .Include(p => p.GeoCoordinate)
                        .ToListAsync();
                }
            }

            return new OkObjectResult(districts);
        }

        [HttpGet]
        [Route("[controller]/{slug}")]
        public async Task<IActionResult> Province([FromRoute] string slug)
        {
            using (var context = new CovidContext())
            {
                Province province = await context.Provinces
                    .Where(p => p.SlugId == slug)
                    .Include(p => p.Country)
                    .Include(p => p.GeoCoordinate)
                    .FirstOrDefaultAsync();

                province.TimeSeries = await GetTimeSeriesStatistics(context.DataPoints, province);
                return new OkObjectResult(province);
            }
        }

        protected async Task<List<Total>> GetTimeSeriesStatistics(DbSet<DataPoint> dbSet, Entity entity)
        {
            var query = await dbSet.Where(dp => dp.ProvinceSlugId == entity.Id)
                        .GroupBy(dp => dp.SourceFile)
                        .Where(s => s.Count() >= 0)
                        .OrderBy(s => s.Key)
                        .Select(s => new Total()
                        {
                            SourceFile = s.Key,
                            Deaths = (int)s.Sum(x => x.Deaths),
                            Confirmed = (int)s.Sum(x => x.Deaths),
                            Recovered = (int)s.Sum(x => x.Deaths),
                            Active = (int)s.Sum(x => x.Active),
                            Count = s.Count()
                        }).ToListAsync();
            return query;
        }
    }
}
