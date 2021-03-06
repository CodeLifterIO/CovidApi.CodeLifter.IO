//using System;
//using System.IO;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Azure.WebJobs;
//using Microsoft.Azure.WebJobs.Extensions.Http;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
//using AzureFunctions.Extensions.Swashbuckle.Attribute;
//using AzureFunctions.Extensions.Swashbuckle;
//using System.Net.Http;

//namespace CodeLifter.IO.CovidApi.Functions.Controllers
//{
//    public static class SwaggerController
//    {
//        [FunctionName("SwaggerJson")]
//        [SwaggerIgnore]
//        public static Task<HttpResponseMessage> Run(
//            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "json")]
//            HttpRequestMessage req,
//            ILogger log,
//            [SwashBuckleClient] ISwashBuckleClient swashBuckleClient)
//        {
//            return Task.FromResult(swashBuckleClient.CreateSwaggerDocumentResponse(req));
//        }

//        [FunctionName("SwaggerUI")]
//        [SwaggerIgnore]
//        public static Task<HttpResponseMessage> RunUI(
//            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "/")]
//            HttpRequestMessage req,
//            ILogger log,
//            [SwashBuckleClient] ISwashBuckleClient swashBuckleClient)
//        {
//            return Task.FromResult(swashBuckleClient.CreateSwaggerUIResponse(req, "json"));
//        }
//    }
//}
