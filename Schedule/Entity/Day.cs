using Schedule.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schedule.Entity
{
    public class Day
    {
        public string Time { get; set; }
        public string Name { get; set; }
        public bool Current { get; set; }
        public IEnumerable<ScheduleTime> Morning { get; set; }
        public IEnumerable<ScheduleTime> Afternoon { get; set; }
    }
}