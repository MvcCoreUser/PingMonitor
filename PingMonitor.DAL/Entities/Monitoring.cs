using System;
using System.Collections.Generic;
using System.Text;

namespace PingMonitor.DAL.Entities
{
    public class Monitoring
    {
        public long Id { get; set; }
        public int ApiInfoId { get; set; }
        public ApiInfo ApiInfo { get; set; }
        public DateTime? ExecStart { get; set; }
        public long? ExecTimeInMilliseconds { get; set; }
        public bool Accessible { get; set; }
    }
}
