using System;
using System.Collections.Generic;
using System.Text;

namespace PingMonitor.BLL.Infrastructure
{
    public class OperationResult
    {
        public bool Succeded { get; set; }
        public string Message { get; set; }
        public object Tag { get; set; }
        public Exception Exception { get; set; }

    }
}
