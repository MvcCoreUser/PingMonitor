using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PingMonitor.BLL.Infrastructure;
using PingMonitor.BLL.Interfaces;
using PingMonitor.BLL.ViewModels;
using PingMonitor.DAL.Entities;
using PingMonitor.DAL.Interfaces;

namespace PingMonitor.BLL.Services
{
    public class MonitoringService : IMonitoringService
    {
        public IRepositoryContext Database { get; set; }
        public IConfiguration Configuration { get; }
        public MonitoringService(IRepositoryContext db, IConfiguration configuration)
        {
            Database = db;
            Configuration = configuration;
        }
        public async Task<IEnumerable<ResultViewModel>> GetResults(int? apiId=null)
        {
            int standardExecTime = int.Parse(Configuration["Params:StardardExecTime"]);
            List<ResultViewModel> results = new List<ResultViewModel>();
            var apiInfos = apiId.HasValue ? Database.ApiInfoRepo.Get(ai => ai.Id.Equals(apiId.Value)) : Database.ApiInfoRepo.GetAll();
            apiInfos.ToList().ForEach(api =>
            {
                ResultViewModel result = new ResultViewModel()
                {
                    ApiServiceViewModel = ApiInfoViewModel.FromEntity(api),
                    StandardExecTime = standardExecTime
                };
                results.Add(result);
            });

            foreach (var result in results)
            {
                var checkApi = await this.RunMonitoring(result.ApiServiceViewModel);
                result.Accessible = checkApi.Accessible;
                if (result.ApiServiceViewModel.CheckExecTime)
                {
                    result.ExecTimeInSeconds = checkApi.ExecTimeInMilliseconds;

                    if (apiId.HasValue)
                    {
                        var filteredMonitoringsForHour = Database.MonitoringRepo.Get(m => m.ApiInfoId.Equals(result.ApiServiceViewModel.Id))
                                                                .Where(m => (DateTime.Now - m.ExecStart.Value).TotalMinutes <= 60);
                        result.MaxExecTimePerLastHour = filteredMonitoringsForHour
                                                                .Any(m => m.ExecTimeInMilliseconds.Value / 2.0 > result.StandardExecTime)
                                                                ? filteredMonitoringsForHour.Max(m => m.ExecTimeInMilliseconds)
                                                                : null;
                        var filteredMonitoringsForDay = Database.MonitoringRepo.Get(m => m.ApiInfoId.Equals(result.ApiServiceViewModel.Id))
                                                                    .Where(m => (DateTime.Now - m.ExecStart.Value).TotalHours <= 24);
                        result.MaxExecTimePerLastDay = filteredMonitoringsForDay
                                                                    .Any(m => m.ExecTimeInMilliseconds.Value / 2.0 > result.StandardExecTime)
                                                                    ? filteredMonitoringsForDay.Max(m => m.ExecTimeInMilliseconds)
                                                                    : null;
                    }
                    
                    


                }
                result.FailCountPerLastHour = Database.MonitoringRepo.Get(m => m.ApiInfoId.Equals(result.ApiServiceViewModel.Id))
                                                        .Count(m => !m.Accessible && (DateTime.Now - m.ExecStart.Value).TotalMinutes <= 60);
                result.FailCountPerLastDay = Database.MonitoringRepo.Get(m => m.ApiInfoId.Equals(result.ApiServiceViewModel.Id))
                                                    .Count(m => !m.Accessible && (DateTime.Now - m.ExecStart.Value).TotalHours <= 24);

            }
            
            return results;
        }

        public async Task<OperationResult> SeedApiInfoData(IEnumerable<ApiInfoViewModel> apiInfoViewModels)
        {
            var apiServices = apiInfoViewModels.Select(a => a.ToEntity(Database)).ToList();
            try
            {
                foreach (var api in apiServices)
                {
                    await Database.ApiInfoRepo.CreateAsync(api);
                }
                return new OperationResult()
                {
                    Succeded = true,
                    Message = "Добавление api-сервисов прошло успешно"
                };
            }
            catch (Exception ex)
            {

                return new OperationResult()
                {
                    Succeded = false,
                    Message = ex.Message,
                    Exception= ex
                };
            }
            
            
        }

        public async Task<Monitoring> RunMonitoring(ApiInfoViewModel apiInfoViewModel)
        {
            Monitoring res = new Monitoring();
            HttpClient httpClient = new HttpClient();
            if (apiInfoViewModel.CheckExecTime)
            {
                httpClient.DefaultRequestHeaders.Add("AccessKey", "test_05fc5ed1-0199-4259-92a0-2cd58214b29c");
            }
            Stopwatch timer = new Stopwatch();
            res.ExecStart = DateTime.Now;
            try
            {
                timer.Start();
                var response = await httpClient.GetAsync(apiInfoViewModel.Url);
                timer.Stop();
                if (apiInfoViewModel.CheckExecTime)
                {
                    res.ExecTimeInMilliseconds = timer.ElapsedMilliseconds;
                }
                res.Accessible = response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                timer.Stop();
                if (apiInfoViewModel.CheckExecTime)
                {
                    res.ExecTimeInMilliseconds = null;
                }
                res.Accessible = false;
            }
           
            
            res.ApiInfoId = apiInfoViewModel.Id;
            return res;
        }

        public async Task RecuringCheckApiJob()
        {
            foreach (var apiInfo in Database.ApiInfoRepo.GetAll().ToArray())
            {
                var monitoring = await RunMonitoring(ApiInfoViewModel.FromEntity(apiInfo));
                await Database.MonitoringRepo.CreateAsync(monitoring);
            }
        }
    }
}
