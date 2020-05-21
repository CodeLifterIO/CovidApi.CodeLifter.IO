using System;
using System.Text.Json;
using CodeLifter.Covid19.Data;
using CovidApi.CodeLifter.IO.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CovidApi.CodeLifter.IO
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "CovidApi.Codelifter.IO",
                        Version = "v1",
                        Description = @"An easy to use API to track the number sof the growing SARS-CoV-2 novel Coronavirus pandemic.  All unique location endpoints return current data, totals, and timeseries.",
                        Contact = new OpenApiContact
                        {
                            Name = "Andrew Palmer (CodeLifterIO)",
                            Email = "a@codelifter.net"
                        },
                        License = new OpenApiLicense
                        {
                            Name = "MIT License",
                            Url = new Uri("https://www.mit.edu/~amini/LICENSE.md")
                        }
                    });
            });

            services.AddControllers(options =>
                    {
                        options.Filters.Add(typeof(PasswordResourceFilter));
                    })
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.IgnoreNullValues = true;
                    });
            services.AddDbContext<CovidContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            UpdateDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CovidApi.Codelifter.IO");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<CovidContext>())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
