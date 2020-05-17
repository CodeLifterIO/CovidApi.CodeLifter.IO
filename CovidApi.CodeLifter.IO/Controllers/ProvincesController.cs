//using System;
//using System.IO;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Azure.WebJobs;
//using Microsoft.Azure.WebJobs.Extensions.Http;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
//using CodeLifter.Covid19.Data;
//using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using CodeLifter.Covid19.Data.Models;

//namespace CovidApi.CodeLifter.IO.Controllers
//{
//    public class ProvincesController : BaseController
//    {
//        [FunctionName("HttpTrigger_Provinces_GetAll")]
//        public async Task<HttpResponseMessage> Provinces(
//            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "provinces")] HttpRequest req,
//            ILogger log)
//        {
//            using (var context = new CovidContext())
//            {
//                List<Province> provinces = await context.Provinces
//                    .Include(p => p.GeoCoordinate)
//                    .Include(p => p.Country)
//                    .ToListAsync();
//                return ConvertToJsonAndReturnOK(provinces);
//            }
//        }

//        [FunctionName("HttpTrigger_Province_Get")]
//        public async Task<HttpResponseMessage> Province(
//            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "province/{slug}")] HttpRequest req,
//            string slug,
//            ILogger log)
//        {
//            using (var context = new CovidContext())
//            {
//                Province province = await context.Provinces
//                    .Where(p => p.Slug == slug)
//                    .Include(p => p.GeoCoordinate)
//                    .Include(p => p.Country)
//                    .FirstOrDefaultAsync();

//                province.CurrentData = await GetLatestStatistic(context.DataPoints, province);
//                return ConvertToJsonAndReturnOK(province);
//            }
//        }

//        [FunctionName("HttpTrigger_Province_GetDistricts")]
//        public async Task<HttpResponseMessage> GetDistrictsByProvince(
//            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "province/{slug}/districts")] HttpRequest req,
//            string slug,
//            ILogger log)
//        {
//            Province province;
//            using (var context = new CovidContext())
//            {
//                province = await context.Provinces
//                    .Where(p => p.Slug == slug)
//                    .Include(p => p.GeoCoordinate)
//                    .Include(p => p.Country)
//                    .FirstOrDefaultAsync();
//            }


//            List<District> districts = null;
//            if (null != province)
//            {
//                using (var context = new CovidContext())
//                {
//                    districts = await context.Districts
//                        .Where(p => p.ProvinceId == province.Id)
//                        .Include(p => p.Country)
//                        .Include(p => p.Province)
//                        .Include(p => p.GeoCoordinate)
//                        .ToListAsync();
//                }
//            }

//            return ConvertToJsonAndReturnOK(districts);
//        }


//        [FunctionName("HttpTrigger_Province_GetTimeSeries")]
//        public async Task<HttpResponseMessage> GetTimeSeriesByProvince(
//            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "province/{slug}/timeseries")] HttpRequest req,
//            string slug,
//            ILogger log)
//        {
//            using (var context = new CovidContext())
//            {
//                Province province = await context.Provinces
//                    .Where(p => p.Slug == slug)
//                    .Include(p => p.Country)
//                    .Include(p => p.GeoCoordinate)
//                    .FirstOrDefaultAsync();

//                province.TimeSeries = await GetTimeSeriesStatistics(context.DataPoints, province);
                
//                return ConvertToJsonAndReturnOK(province);
//            }
//        }

//        protected async Task<Statistic> GetLatestStatistic(DbSet<DataPoint> dbSet, Entity entity)
//        {
//            return await dbSet.Where(dp => dp.ProvinceId == entity.Id)
//                        .GroupBy(dp => dp.SourceFile)
//                        .Where(s => s.Count() >= 0)
//                        .OrderBy(s => s.Key)
//                        .Select(s => new Statistic()
//                        {
//                            SourceFile = s.Key,
//                            Deaths = (int)s.Sum(x => x.Deaths),
//                            Confirmed = (int)s.Sum(x => x.Deaths),
//                            Recovered = (int)s.Sum(x => x.Deaths),
//                            Active = (int)s.Sum(x => x.Active),
//                            Count = s.Count()
//                        }).LastAsync();
//        }

//        protected async Task<List<Statistic>> GetTimeSeriesStatistics(DbSet<DataPoint> dbSet, Entity entity)
//        {
//            var query = await dbSet.Where(dp => dp.ProvinceId == entity.Id)
//                        .GroupBy(dp => dp.SourceFile)
//                        .Where(s => s.Count() >= 0)
//                        .OrderBy(s => s.Key)
//                        .Select(s => new Statistic()
//                        {
//                            SourceFile = s.Key,
//                            Deaths = (int)s.Sum(x => x.Deaths),
//                            Confirmed = (int)s.Sum(x => x.Deaths),
//                            Recovered = (int)s.Sum(x => x.Deaths),
//                            Active = (int)s.Sum(x => x.Active),
//                            Count = s.Count()
//                        }).ToListAsync();
//            return query;
//        }

//    }
//}
