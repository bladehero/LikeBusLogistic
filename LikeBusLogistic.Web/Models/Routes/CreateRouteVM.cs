using LikeBusLogistic.VM.ViewModels;
using System.Collections.Generic;

namespace LikeBusLogistic.Web.Models.Routes
{
    public class CreateRouteVM
    {
        public LocationVM StartLocation { get; set; }

        public string Name { get; set; }
        public float? EstimatedDurationInHours { get; set; }
        public List<LocationVM> Locations { get; set; }

        public CreateRouteVM()
        {
            Locations = new List<LocationVM>();
        }
    }
}
