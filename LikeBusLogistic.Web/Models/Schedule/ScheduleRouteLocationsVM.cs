using LikeBusLogistic.VM.ViewModels;
using System.Collections.Generic;

namespace LikeBusLogistic.Web.Models.Schedule
{
    public class ScheduleRouteLocationsVM
    {
        public IEnumerable<ScheduleRouteLocationVM> ScheduleRouteLocations { get; set; }
        public bool HideAutoSelect { get; set; }
        public bool HasConfirmedTrips { get; set; }
        public bool NeedsSync { get; set; }

        public ScheduleRouteLocationsVM()
        {
            ScheduleRouteLocations = new List<ScheduleRouteLocationVM>();
        }
    }
}
