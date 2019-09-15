using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PingMonitor.BLL.Interfaces;
using PingMonitor.BLL.ViewModels;
using PingMonitor.DAL.EF;

namespace PingMonitor.Web.Infrastructure
{
    public class Seed
    {
        public static async Task SeedDatabase(IServiceProvider serviceProvider)
        {
            var context= serviceProvider.GetRequiredService<AppDbContext>();
            context.Database.Migrate();
            if (!context.ApiInfos.Any())
            {
                List<ApiInfoViewModel> apiInfos = new List<ApiInfoViewModel>()
                {
                    new ApiInfoViewModel()
                    {
                        Url= "https://jsonplaceholder.typicode.com/posts/1/comments",
                        CheckExecTime=false
                    },
                    new ApiInfoViewModel()
                    {
                        Url = "https://jsonplaceholder.typicode.com/albums/1/photos",
                        CheckExecTime=false
                    },
                    new ApiInfoViewModel()
                    {
                        Url="https://jsonplaceholder.typicode.com/users/1/albums",
                        CheckExecTime = true
                    }
                };

                await serviceProvider.GetRequiredService<IMonitoringService>().SeedApiInfoData(apiInfos);
            }

        }    
    }
}
