using System;
using System.Collections.Generic;
using System.Text;
using PingMonitor.DAL.EF;
using PingMonitor.DAL.Entities;
using PingMonitor.DAL.Interfaces;

namespace PingMonitor.DAL.Repositories
{
    public class ApiInfoRepo : BaseRepository<ApiInfo>, IApiInfoRepo
    {
        public ApiInfoRepo(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
