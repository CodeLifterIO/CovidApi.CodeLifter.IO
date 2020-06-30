using System.Threading.Tasks;
using CodeLifter.Covid19.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using CodeLifter.Covid19.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Octokit.Internal;
using Microsoft.AspNetCore.Http;
using System;

namespace CovidApi.CodeLifter.IO.Controllers
{
    public class CountryController : BaseController
    {
        [HttpGet]
        [Route("[controller]/{slug}/[action]")]
        public async Task<IActionResult> Provinces([FromRoute] string slug, [FromQuery] string searchTerm = "")
        {
            Country country = null;
            using (var context = new CovidContext())
            {
                country = await context.Countries
                    .Where(c => c.SlugId == slug)
                    .Include(c => c.GeoCoordinate)
                    .FirstOrDefaultAsync();

                var query = from dp in context.Set<DataPoint>()
                            where dp.CountrySlugId == country.SlugId
                            group dp by dp.SourceFile into s
                            where s.Count() > 0
                            orderby s.Key
                            select new Total()
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
                    var query = context.Provinces
                        .Where(p => p.CountrySlugId == country.SlugId);

                    if (!string.IsNullOrWhiteSpace(searchTerm))
                    {
                        query = query.Where(p => p.Name.Contains(searchTerm) || p.SlugId.Contains(searchTerm));
                    }

                    provinces = await query
                        //.Include(p => p.Country)
                        .Include(p => p.GeoCoordinate)
                        .ToListAsync();
                }
            }

            return new OkObjectResult(provinces);
        }


        [HttpGet]
        [Route("[controller]/{slug}")]
        public async Task<IActionResult> Data([FromRoute] string slug)
        {
            Country country = null;
            using (var context = new CovidContext())
            {
                country = await context.Countries
                    .Where(c => c.SlugId == slug)
                    .Include(c => c.GeoCoordinate)
                    .FirstOrDefaultAsync();


                var query = from dp in context.Set<DataPoint>()
                            where dp.CountrySlugId == country.SlugId
                            group dp by dp.SourceFile into s
                            where s.Count() > 0
                            orderby s.Key
                            select new Total()
                            {
                                SourceFile = s.Key,
                                Deaths = (int)s.Sum(x => x.Deaths),
                                Confirmed = (int)s.Sum(x => x.Deaths),
                                Recovered = (int)s.Sum(x => x.Deaths),
                                Active = (int)s.Sum(x => x.Active),
                                Count = s.Count()
                            };
                //country.TimeSeries = query.ToList();
                return new OkObjectResult(country);
            }
        }
    }
}
