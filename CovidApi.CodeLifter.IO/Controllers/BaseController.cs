using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CovidApi.CodeLifter.IO.Controllers
{
    public class BaseController
    {

        public HttpResponseMessage ConvertToJsonAndReturnOK(object entity)
        {
            return ConvertToJsonAndReturnStatus(entity, HttpStatusCode.OK);
        }

        public HttpResponseMessage ConvertToJsonAndReturnStatus(object entity, HttpStatusCode responseCode = HttpStatusCode.OK)
        {
            string json = JsonConvert.SerializeObject(entity);

            return new HttpResponseMessage(responseCode)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
        }
    }
}
