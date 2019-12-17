using LikeBusLogistic.VM.ViewModels;
using System.Collections.Generic;

namespace LikeBusLogistic.Web.Models.Routes
{
    public class MergeRouteLocationVM
    {
        public RouteLocationVM RouteLocation { get; set; }
        public LocationVM LocationToAdd { get; set; }
        public IEnumerable<LocationVM> Locations { get; set; }
        public IEnumerable<RouteLocationVM> RouteLocations { get; set; }
        public MergeRouteLocationMode Mode { get; set; }

        public MergeRouteLocationVM()
        {
            Locations = new List<LocationVM>();
            RouteLocations = new List<RouteLocationVM>();
        }
    }
}
