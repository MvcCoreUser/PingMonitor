using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PingMonitor.DAL.EF;
using PingMonitor.DAL.Entities;
using PingMonitor.DAL.Interfaces;

namespace PingMonitor.DAL.Repositories
{
    public class RepositoryContext : IRepositoryContext
    {
        private AppDbContext context;
        private bool disposed = false;

        public RepositoryContext(AppDbContext dbContext)
        {
            context = dbContext;
            ApiInfoRepo = new ApiInfoRepo(context);
            MonitoringRepo = new MonitoringRepo(context);
            
        }

        public IMonitoringRepo MonitoringRepo { get; }

        public IApiInfoRepo ApiInfoRepo { get; }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    MonitoringRepo.Dispose();
                    ApiInfoRepo.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<int> SaveAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}
