using Logistic.VM.ViewModels;
using System.Collections.Generic;

namespace Logistic.Web.Models.Routes
{
    public class MergeRouteLocationVM
    {
        public int RouteId { get; set; }
        public int RouteLocationId { get; set; }
        public int LocationToAddId { get; set; }
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
