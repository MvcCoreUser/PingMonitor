using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PingMonitor.BLL.ViewModels
{
    public class ResultViewModel
    {
        public ApiInfoViewModel  ApiServiceViewModel { get; set; }
        public int StandardExecTime { get; set; }
        public long? ExecTimeInSeconds { get; set; }
        public bool LargeVariance
            => ExecTimeInSeconds.HasValue ? (double)(ExecTimeInSeconds.Value / StandardExecTime) > 2 : false;

        public bool Accessible { get; set; }


        public int? FailCountPerLastHour { get; set; }
        public long? MaxExecTimePerLastHour { get; set; }
        //    =>ExecTimes.Average() /StandardExecTime > 2 ?(int?)ExecTimes.Max() : null;
        public int? FailCountPerLastDay { get; set; }
        

        public long? MaxExecTimePerLastDay { get; set; }
            //(int[] ExecTimes)
            //=> ExecTimes.Average()/ StandardExecTime > 2 ? (int?)ExecTimes.Max() : null;
    }
       
}
