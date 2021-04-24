using Logistic.VM.ViewModels;
using System.Collections.Generic;

namespace Logistic.Web.Models.Routes
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
