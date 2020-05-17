//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Threading.Tasks;
//using CodeLifter.Covid19.Data;
//using CodeLifter.Covid19.Data.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Azure.WebJobs;
//using Microsoft.Azure.WebJobs.Extensions.Http;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;

//namespace CovidApi.CodeLifter.IO.Controllers
//{
//    public class GlobalController : BaseController
//    {
//        [FunctionName("HttpTrigger_Global_Get_TimeSeries")]
//        public async Task<HttpResponseMessage> GlobalTimeSeries(
//            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "global/timeseries")] HttpRequest req,
//            ILogger log)
//        {
//            using (var context = new CovidContext())
//            {
//                Planet earth = new Planet();
//                var query = from dp in context.Set<DataPoint>()
//                            group dp by dp.SourceFile into s
//                            where s.Count() > 0
//                            orderby s.Key
//                            select new Statistic()
//                            {
//                                SourceFile = s.Key,
//                                Deaths    = (int)s.Sum(x => x.Deaths),
//                                Confirmed = (int)s.Sum(x => x.Deaths),
//                                Recovered = (int)s.Sum(x => x.Deaths),
//                                Active    = (int)s.Sum(x => x.Active),
//                                Count     = s.Count()
//                            };
//                earth.TimeSeries = query;
//                return ConvertToJsonAndReturnOK(earth);
//            }
//        }

//        [FunctionName("HttpTrigger_Global_Get_Current")]
//        public async Task<HttpResponseMessage> GlobalCurrent(
//            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "global")] HttpRequest req,
//            ILogger log)
//        {
//            using (var context = new CovidContext())
//            {
//                Planet earth = new Planet();
//                var query = from dp in context.Set<DataPoint>()
//                            group dp by dp.SourceFile into s
//                            where s.Count() > 0
//                            orderby s.Key
//                            select new Statistic()
//                            {
//                                SourceFile = s.Key,
//                                Deaths = (int)s.Sum(x => x.Deaths),
//                                Confirmed = (int)s.Sum(x => x.Deaths),
//                                Recovered = (int)s.Sum(x => x.Deaths),
//                                Active = (int)s.Sum(x => x.Active),
//                                Count = s.Count()
//                            };
//                earth.CurrentData = query.Last();
//                return ConvertToJsonAndReturnOK(earth);
//            }
//        }
//    }
//}
