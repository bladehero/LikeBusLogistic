using LikeBusLogistic.VM.ViewModels;
using System.Collections.Generic;

namespace LikeBusLogistic.Web.Models.Routes
{
    public class RoutesVM
    {
        public IEnumerable<RouteVM> Routes { get; set; }

        public RoutesVM()
        {
            Routes = new List<RouteVM>();
        }
    }
}
