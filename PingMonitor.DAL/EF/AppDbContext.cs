using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PingMonitor.DAL.Entities;

namespace PingMonitor.DAL.EF
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) 
        {
        }

        public DbSet<ApiInfo> ApiInfos{ get; set; }
        public DbSet<Monitoring> Monitorings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
