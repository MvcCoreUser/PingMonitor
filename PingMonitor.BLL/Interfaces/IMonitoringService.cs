using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PingMonitor.BLL.Infrastructure;
using PingMonitor.BLL.ViewModels;
using PingMonitor.DAL.Entities;

namespace PingMonitor.BLL.Interfaces
{
    public interface IMonitoringService
    {
        Task<OperationResult> SeedApiInfoData(IEnumerable<ApiInfoViewModel> apiInfoViewModels);
        Task<IEnumerable<ResultViewModel>> GetResults(int? apiId=null);
        Task<Monitoring> RunMonitoring(ApiInfoViewModel apiInfoViewModel);
        Task RecuringCheckApiJob();
    }
}
