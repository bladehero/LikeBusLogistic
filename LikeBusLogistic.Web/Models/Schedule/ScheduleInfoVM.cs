using LikeBusLogistic.VM.ViewModels;
using System;
using System.Collections.Generic;

namespace LikeBusLogistic.Web.Models.Schedule
{
    public class ScheduleInfoVM
    {
        public VM.ViewModels.ScheduleVM Schedule { get; set; }
        public IEnumerable<ScheduleRouteLocationVM> ScheduleRouteLocations { get; set; }
        public ScheduleRouteLocationVM First { get; set; }
        public ScheduleRouteLocationVM Last { get; set; }
        public double TotalDistance { get; set; }
        public TimeSpan TotalTime { get; set; }
        public bool IsPrint { get; set; }

        public ScheduleInfoVM()
        {
            ScheduleRouteLocations = new List<ScheduleRouteLocationVM>();
        }
    }
}
