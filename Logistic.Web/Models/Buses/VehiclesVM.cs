using Logistic.VM.ViewModels;
using System.Collections.Generic;

namespace Logistic.Web.Models.Buses
{
    public class VehiclesVM
    {
        public IEnumerable<VehicleVM> Vehicles { get; set; }

        public VehiclesVM()
        {
            Vehicles = new List<VehicleVM>();
        }
    }
}
