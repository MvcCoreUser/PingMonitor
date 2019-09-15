using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PingMonitor.BLL.Interfaces;
using PingMonitor.BLL.Services;
using PingMonitor.BLL.ViewModels;
using PingMonitor.DAL.EF;
using PingMonitor.DAL.Interfaces;
using PingMonitor.DAL.Repositories;
using PingMonitor.Web.Infrastructure;

namespace PingMonitor
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
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(Configuration["ConnectionStrings:Default"], 
                    pgOptions=> { pgOptions.MigrationsAssembly("PingMonitor.Web"); });
            });

            services.AddTransient<IRepositoryContext, RepositoryContext>();
            services.AddTransient<IMonitoringService, MonitoringService>();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            PostgreSqlStorageOptions hfStorageOptions = new PostgreSqlStorageOptions()
            {
                PrepareSchemaIfNecessary = true,
                SchemaName = "public"
            };
            services.AddHangfire(config => config.UsePostgreSqlStorage(Configuration["ConnectionStrings:HangFire"], hfStorageOptions));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            app.UseHangfireDashboard("/hangfire");
            app.UseHangfireServer();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            using (var scope = app.ApplicationServices.CreateScope())
            {
                Seed.SeedDatabase(scope.ServiceProvider).GetAwaiter().GetResult();
                RecurringJob.AddOrUpdate(() => scope.ServiceProvider.GetRequiredService<IMonitoringService>().RecuringCheckApiJob(), Cron.Minutely());
            }
            
            
            
        }
    }
}
