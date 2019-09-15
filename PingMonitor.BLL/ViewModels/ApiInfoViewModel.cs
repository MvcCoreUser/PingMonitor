using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PingMonitor.DAL.EF;
using PingMonitor.DAL.Entities;
using PingMonitor.DAL.Interfaces;

namespace PingMonitor.BLL.ViewModels
{
    public class ApiInfoViewModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool CheckExecTime { get; set; }
        public static ApiInfoViewModel FromEntity(ApiInfo apiService)
        {
            return new ApiInfoViewModel()
            {
                Id = apiService.Id,
                Url = apiService.Url,
                CheckExecTime = apiService.CheckExecTime
            };
        }

        public ApiInfo ToEntity(IRepositoryContext context)
        {
            if (context.ApiInfoRepo.FindById(this.Id)==null)
            {
                return new ApiInfo()
                {
                    Id = default(int),
                    Url = this.Url,
                    CheckExecTime = this.CheckExecTime
                };
            }
            return context.ApiInfoRepo.GetWithInclude(api => api.Id.Equals(this.Id), api => api.Monitorings).FirstOrDefault();
        }
         
    }
}
