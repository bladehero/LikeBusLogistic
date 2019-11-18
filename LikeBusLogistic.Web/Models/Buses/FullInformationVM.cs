using LikeBusLogistic.VM.ViewModels;
using System.Collections.Generic;

namespace LikeBusLogistic.Web.Models.Buses
{
    public class FullInformationVM
    {
        public IEnumerable<BusVM> Buses { get; set; }
        public IEnumerable<VehicleVM> Vehicles { get; set; }

        public FullInformationVM()
        {
            Buses = new List<BusVM>();
            Vehicles = new List<VehicleVM>();
        }
    }
}
