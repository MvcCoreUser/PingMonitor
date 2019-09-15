using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PingMonitor.DAL.Entities
{
    public class ApiInfo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool CheckExecTime { get; set; }
        public ICollection<Monitoring> Monitorings { get; set; } = new List<Monitoring>();
    }
}
