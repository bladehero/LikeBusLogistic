using LikeBusLogistic.VM.ViewModels;
using System.Collections.Generic;

namespace LikeBusLogistic.Web.Models.Trips
{
    public class TripsVM
    {
        public TripTab Tab { get; set; }
        public IEnumerable<TripVM> Trips { get; set; }

        public TripsVM()
        {
            Trips = new List<TripVM>();
        }
    }
}
