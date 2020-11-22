using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using CovidApi.Data;
using CovidApi.Infrastructure;
using CovidApi.Infrastructure.ApplicationUserClaims;
using CovidApi.Infrastructure.AppSettingsModels;
using CovidApi.Models;
using CovidApi.Infrastructure.Startup;
using CovidApi.Repositories;
using CovidApi.Services;
using CovidApi.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Octokit;
using System.Net;
using CovidApi.Data.Repositories;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using Amazon.S3;

namespace CovidApi
{
    public class Startup
    {
        public IWebHostEnvironment Env { get; }
        public IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            Env = env;
        }

        public void ConfigureDataRepositories(IServiceCollection services)
        {
            services.AddScoped<IDatabaseRepository, DatabaseRepository>();
            services.AddScoped<IDataFileRepository, DataFileRepository>();
        }

        public void ConfigureAppServices(IServiceCollection services)
        {
            services.AddScoped<IEnvironmentService, EnvironmentService>();
            services.AddScoped<IDataUpdateService, DataUpdateService>();
            services.AddScoped<Slugify.ISlugHelper, Slugify.SlugHelper>();
            
            services.AddHttpClient<IGithubService, GithubService>(client =>
                {
                    client.BaseAddress = new Uri("https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/");
                });
            services.AddScoped<IGithubService, GithubService>();

            //Octokit.GitHubClient octo = new GitHubClient(r);
            //services.AddSingleton<Octokit.IGitHubClient, Octokit.GitHubClient>();

            //services.AddScoped<IWebClient, WebClient>();
            //services.AddSingleton<IGithubService, GithubService>();

        }

        public void ConfigureBuiltInServices(IServiceCollection services)
        {
            //built into ASPNet
            services.AddHttpContextAccessor();
            services.AddLogging();
        }

        public void ConfigureSettingsAndKeys(IServiceCollection services)
        {
            services.Configure<ConnectionStrings>(_configuration.GetSection(nameof(ConnectionStrings)));
            services.Configure<GithubSettings>(_configuration.GetSection(nameof(GithubSettings)));
            services.Configure<EmailSettings>(_configuration.GetSection(nameof(EmailSettings)));
            services.Configure<ScriptTags>(_configuration.GetSection(nameof(ScriptTags)));
            services.Configure<LoggingSettings>(_configuration.GetSection("Logging"));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDefaultAWSOptions(_configuration.GetAWSOptions());
            services.AddAWSService<IAmazonS3>();

            ConfigureSettingsAndKeys(services);
            ConfigureBuiltInServices(services);
            ConfigureDataRepositories(services);
            ConfigureAppServices(services);

            services.AddSingleton<IEmailSender, EmailSender>();

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed
                // for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
            });

            services.AddDbContextPool<CovidContext>(ctx => ctx.UseNpgsql(_configuration.GetConnectionString("DatabaseConnection")));

            services.AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<CovidContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();

            services.Configure<IdentityOptions>(options =>
            {
                // Default Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // Default Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;

                // Default SignIn settings.
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                // Default User settings.
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            services.AddSwaggerGen(swagger =>
            {
                swagger.DescribeAllParametersInCamelCase();
                swagger.SwaggerDoc("v1",
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

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "CreativeTim.Argon.DotNetCore.AppCookie";
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                // You might want to only set the application cookies over a secure connection:
                // options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.ExpireTimeSpan = TimeSpan.FromDays(365);
                options.SlidingExpiration = true;
            });

            // As per https://github.com/aspnet/AspNetCore/issues/5828
            // the settings for the cookie would get overwritten if using the default UI so
            // we need to "post-configure" the authentication cookie
            services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, options =>
            {
                options.AccessDeniedPath = "/access-denied";
                options.LoginPath = "/login";
                options.LogoutPath = "/logout";

                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
            });

            services.AddDataProtection()
                .PersistKeysToDbContext<CovidContext>();

            services.AddAntiforgery();

            

            services.AddControllersWithViews(options =>
            { 
                // Slugify routes so that we can use /employee/employee-details/1 instead of
                // the default /Employee/EmployeeDetails/1
                //
                // Using an outbound parameter transformer is a better choice as it also allows
                // the creation of correct routes using view helpers
                options.Conventions.Add(
                    new RouteTokenTransformerConvention(
                        new SlugifyParameterTransformer()));

                // Enable Antiforgery feature by default on all controller actions
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            services.AddRazorPages(options =>
                {
                    options.Conventions.AddAreaPageRoute("Identity", "/Account/Register", "/register");
                    options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "/login");
                    options.Conventions.AddAreaPageRoute("Identity", "/Account/Logout", "/logout");
                    options.Conventions.AddAreaPageRoute("Identity", "/Account/ForgotPassword", "/forgot-password");
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddSessionStateTempDataProvider();

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

            // You probably want to use in-memory cache if not developing using docker-compose
            services.AddDistributedRedisCache(action => { action.Configuration = _configuration["Redis:InstanceName"]; });

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromDays(365);
                options.Cookie.Name = "CovidApi.CodeLifter.IO";
                // You might want to only set the application cookies over a secure connection:
                // options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });



            // This adds a hosted service that, on application start-up, seeds the database with initial data.
            // You can remove this if you want to prevent the seeding process or you can change the initial data
            // to suit your needs in the IdentityDataSeeder class.
            services.AddHostedService<DbSeederHostedService>();
            services.AddHostedService<LoggingTestHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // This is required to make the application work behind a proxy like NGINX or HAPROXY
            // that also provides TLS termination (switching from incoming HTTPS to HTTP)
            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/status-code", "?code={0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CovidApi.Codelifter.IO API V1");
            });

            app.UseSession();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=home}/{action=index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}");

                endpoints.MapRazorPages();
            });
        }
    }
}
