using LikeBusLogistic.VM.ViewModels;
using System.Collections.Generic;

namespace LikeBusLogistic.Web.Models.Schedule
{
    public class ScheduleRouteLocationsVM
    {
        public IEnumerable<ScheduleRouteLocationVM> ScheduleRouteLocations { get; set; }
        public bool IsModal { get; set; }

        public ScheduleRouteLocationsVM()
        {
            ScheduleRouteLocations = new List<ScheduleRouteLocationVM>();
        }
    }
}
