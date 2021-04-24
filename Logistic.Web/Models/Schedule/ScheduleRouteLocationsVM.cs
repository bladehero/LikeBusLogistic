using Logistic.VM.ViewModels;
using System.Collections.Generic;

namespace Logistic.Web.Models.Schedule
{
    public class ScheduleRouteLocationsVM
    {
        public IEnumerable<ScheduleRouteLocationVM> ScheduleRouteLocations { get; set; }
        public bool HideAutoSelect { get; set; }
        public bool HasConfirmedTrips { get; set; }

        public ScheduleRouteLocationsVM()
        {
            ScheduleRouteLocations = new List<ScheduleRouteLocationVM>();
        }
    }
}
