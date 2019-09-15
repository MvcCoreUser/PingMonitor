using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PingMonitor.DAL.Interfaces
{
    public interface IRepositoryContext : IDisposable
    {
        IMonitoringRepo MonitoringRepo{ get; }
        IApiInfoRepo ApiInfoRepo{ get; }
       
        Task<int> SaveAsync();
    }
}
