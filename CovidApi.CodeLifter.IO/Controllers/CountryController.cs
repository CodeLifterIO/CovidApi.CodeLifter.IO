using System.Threading.Tasks;
using CodeLifter.Covid19.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using CodeLifter.Covid19.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace CovidApi.CodeLifter.IO.Controllers
{
    public class CountryController : BaseController
    {

        [HttpGet]
        [Route("[Action]")]
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
        [Route("[controller]/{slug}/")]
        public async Task<IActionResult> Country([FromRoute]string slug)
        {
            using (var context = new CovidContext())
            {
                Country country = await context.Countries
                    .Where(c => c.Slug == slug)
                    .Include(c => c.GeoCoordinate)
                    .FirstOrDefaultAsync();

                var query = from dp in context.Set<DataPoint>()
                            where dp.CountryId == country.Id
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
                country.CurrentData = query.Last();

                return new OkObjectResult(country);
            }
        }


        [HttpGet]
        [Route("[controller]/{slug}/[action]")]
        public async Task<IActionResult> Provinces([FromRoute]string slug)
        {
            Country country = null;
            using (var context = new CovidContext())
            {
                country = await context.Countries
                    .Where(c => c.Slug == slug)
                    .Include(c => c.GeoCoordinate)
                    .FirstOrDefaultAsync();

                var query = from dp in context.Set<DataPoint>()
                            where dp.CountryId == country.Id
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
            }

            List<Province> provinces = null;
            if (null != country)
            {
                using (var context = new CovidContext())
                {
                    provinces = await context.Provinces
                        .Where(p => p.CountryId == country.Id)
                        .Include(p => p.Country)
                        .Include(p => p.GeoCoordinate)
                        .ToListAsync();
                }
            }

            return new OkObjectResult(provinces);
        }


        [HttpGet]
        [Route("[controller]/{slug}/[action]")]
        public async Task<IActionResult> TimeSeries([FromRoute]string slug)
        {
            Country country = null;
            using (var context = new CovidContext())
            {
                country = await context.Countries
                    .Where(c => c.Slug == slug)
                    .Include(c => c.GeoCoordinate)
                    .FirstOrDefaultAsync();

                var query = from dp in context.Set<DataPoint>()
                            where dp.CountryId == country.Id
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
                country.TimeSeries = query.ToList();

                return new OkObjectResult(country);
            }
        }
    }
}
