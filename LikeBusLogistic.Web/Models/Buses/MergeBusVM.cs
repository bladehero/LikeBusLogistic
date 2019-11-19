using LikeBusLogistic.VM.ViewModels;
using System.Collections.Generic;

namespace LikeBusLogistic.Web.Models.Buses
{
    public class MergeBusVM
    {
        public BusVM Bus { get; set; }
        public IEnumerable<VehicleVM> Vehicles { get; set; }

        public MergeBusVM()
        {
            Vehicles = new List<VehicleVM>();
        }
    }
}
