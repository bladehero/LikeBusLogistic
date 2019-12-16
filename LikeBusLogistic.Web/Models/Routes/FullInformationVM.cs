using LikeBusLogistic.VM.ViewModels;
using System.Collections.Generic;

namespace LikeBusLogistic.Web.Models.Routes
{
    public class FullInformationVM
    {
        public IEnumerable<RouteVM> Routes { get; set; }

        public FullInformationVM()
        {
            Routes = new List<RouteVM>();
        }
    }
}
