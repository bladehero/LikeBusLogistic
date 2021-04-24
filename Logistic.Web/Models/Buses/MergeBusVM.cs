using Logistic.VM.ViewModels;
using System.Collections.Generic;

namespace Logistic.Web.Models.Buses
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
