using AzureFunctions.Extensions.Swashbuckle;
using CodeLifter.Covid19.Data;
using CodeLifter.IO.CovidApi.Functions;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
//using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Swashbuckle.AspNetCore.Swagger;
//using Microsoft.OpenApi.Models;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

[assembly: FunctionsStartup(typeof(CodeLifterFunctionStartup))]
namespace CodeLifter.IO.CovidApi.Functions
{
    class CodeLifterFunctionStartup : FunctionsStartup
    {
        private static void UpdateDatabase(IFunctionsHostBuilder builder)
        {
            //using (var serviceScope = builder.Services
            //    .GetRequiredService<IServiceScopeFactory>()
            //    .CreateScope())
            //{
            //    using (var context = serviceScope.ServiceProvider.GetService<CovidContext>())
            //    {
            //        context.Database.Migrate();
            //    }
            //}


        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            using (var context = new CovidContext())
            {
                context.Database.Migrate();
                Console.Out.WriteLine("*** DB Migrated ***");
            }

            //builder.AddSwashBuckle(Assembly.GetExecutingAssembly());
            builder.Services.AddDbContext<CovidContext>();
            //builder.Services.AddAddJsonOptions(options =>
            //        {
            //            options.JsonSerializerOptions.IgnoreNullValues = true;
            //        });
            //builder.Services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1",
            //        new OpenApiInfo
            //        {
            //            Title = "CovidApi.Codelifter.IO",
            //            Version = "v1",
            //            Description = @"An easy to use API to track the number sof the growing SARS-CoV-2 novel Coronavirus pandemic.  All unique location endpoints return current data, totals, and timeseries.",
            //            Contact = new OpenApiContact
            //            {
            //                Name = "Andrew Palmer (CodeLifterIO)",
            //                Email = "a@codelifter.net"
            //            },
            //            License = new OpenApiLicense
            //            {
            //                Name = "MIT License",
            //                Url = new Uri("https://www.mit.edu/~amini/LICENSE.md")
            //            }
            //        });
            //});
        }
    }
}
