using System;
using System.Collections.Generic;
using System.Text;
using PingMonitor.DAL.EF;
using PingMonitor.DAL.Entities;
using PingMonitor.DAL.Interfaces;

namespace PingMonitor.DAL.Repositories
{
    public class MonitoringRepo : BaseRepository<Monitoring>, IMonitoringRepo
    {
        public MonitoringRepo(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
