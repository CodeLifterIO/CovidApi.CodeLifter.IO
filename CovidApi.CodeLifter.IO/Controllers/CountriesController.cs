using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using CodeLifter.Covid19.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using CodeLifter.Covid19.Data.Models;

namespace CovidApi.CodeLifter.IO.Controllers
{
    public class CountriesController : BaseController
    {
        //public async Task<HttpResponseMessage> GetCountries()
        //{
        //    using (var context = new CovidContext())
        //    {
        //        List<Country> countries = await context.Countries
        //            .Include(country => country.GeoCoordinate)
        //            .ToListAsync();
        //        return ConvertToJsonAndReturnOK(countries);
        //    }
        //}


        //public async Task<HttpResponseMessage> Country(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "country/{slug}")] HttpRequest req,
        //    string slug,
        //    ILogger log)
        //{
        //    using (var context = new CovidContext())
        //    {
        //        Country country = await context.Countries
        //            .Where(c => c.Slug == slug)
        //            .Include(c => c.GeoCoordinate)
        //            .FirstOrDefaultAsync();

        //        var query = from dp in context.Set<DataPoint>()
        //                    where dp.CountryId == country.Id
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
        //        country.CurrentData = query.Last();

        //        return ConvertToJsonAndReturnOK(country);
        //    } 
        //}

        //[FunctionName("HttpTrigger_Country_GetProvinces")]
        //public async Task<HttpResponseMessage> GetProvincesByCountry(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "country/{slug}/provinces")] HttpRequest req,
        //    string slug,
        //    ILogger log)
        //{
        //    Country country = null; 
        //    using (var context = new CovidContext())
        //    {
        //        country = await context.Countries
        //            .Where(c => c.Slug == slug)
        //            .Include(c => c.GeoCoordinate)
        //            .FirstOrDefaultAsync();

        //        var query = from dp in context.Set<DataPoint>()
        //                    where dp.CountryId == country.Id
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
        //    }

        //    List<Province> provinces = null;
        //    if (null != country)
        //    {
        //        using (var context = new CovidContext())
        //        { 
        //            provinces = await context.Provinces
        //                .Where(p => p.CountryId == country.Id)
        //                .Include(p => p.Country)
        //                .Include(p => p.GeoCoordinate)
        //                .ToListAsync();
        //        }
        //    }

        //    return ConvertToJsonAndReturnOK(provinces);
        //}

        //[FunctionName("HttpTrigger_Country_GetTimeSeries")]
        //public async Task<HttpResponseMessage> GetTimeSeriesByCountry(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "country/{slug}/timeseries")] HttpRequest req,
        //    string slug,
        //    ILogger log)
        //{
        //    using (var context = new CovidContext())
        //    {
        //        Country country = await context.Countries
        //            .Where(c => c.Slug == slug)
        //            .Include(c => c.GeoCoordinate)
        //            .FirstOrDefaultAsync();

        //        var query = from dp in context.Set<DataPoint>()
        //                    where dp.CountryId == country.Id
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
        //        country.TimeSeries = query;

        //        return ConvertToJsonAndReturnOK(country);
        //    }
        //}
    }
}
