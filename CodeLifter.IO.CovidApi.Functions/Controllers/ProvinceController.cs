using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CodeLifter.Covid19.Data.Models;
using CodeLifter.Covid19.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CodeLifter.IO.CovidApi.Functions.Controllers
{
    public static class ProvinceController
    {
        [FunctionName("ProvinceDistricts")]
        public async static Task<IActionResult> ProvinceDistricts(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "province/{slug}/districts")] HttpRequest req,
            ILogger log,
            string slug)
        {
            string searchTerm = req.Query["searchTerm"];

            Province province;
            using (var context = new CovidContext())
            {
                province = await context.Provinces
                    .Where(p => p.Slug == slug)
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
                        .Where(p => p.ProvinceId == province.Id);

                    if (!string.IsNullOrWhiteSpace(searchTerm))
                    {
                        query = query.Where(d => d.Name.Contains(searchTerm) || d.Slug.Contains(searchTerm));
                    }

                    districts = await query.Include(p => p.Country)
                        .Include(p => p.GeoCoordinate)
                        .ToListAsync();
                }
            }

            return new OkObjectResult(districts);
        }

        [FunctionName("Province")]
        public async static Task<IActionResult> Province(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "province/{slug}")] HttpRequest req,
            ILogger log,
            string slug)
        {
            using (var context = new CovidContext())
            {
                Province province = await context.Provinces
                    .Where(p => p.Slug == slug)
                    .Include(p => p.GeoCoordinate)
                    .FirstOrDefaultAsync();

                province.TimeSeries = await GetTimeSeriesStatistics(context.DataPoints, province);
                return new OkObjectResult(province);
            }
        }

        async static Task<List<Statistic>> GetTimeSeriesStatistics(DbSet<DataPoint> dbSet, Entity entity)
        {
            var query = await dbSet.Where(dp => dp.ProvinceId == entity.Id)
                        .GroupBy(dp => dp.SourceFile)
                        .Where(s => s.Count() >= 0)
                        .OrderBy(s => s.Key)
                        .Select(s => new Statistic()
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
