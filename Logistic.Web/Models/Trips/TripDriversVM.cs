using Logistic.VM.ViewModels;
using System.Collections.Generic;

namespace Logistic.Web.Models.Trips
{
    public class TripDriversVM
    {
        public int DriversAmount { get; set; }
        public List<int> DriverIds { get; set; }
        public IEnumerable<DriverInfoVM> SelectedDrivers { get; set; }
        public IEnumerable<DriverInfoVM> Drivers { get; set; }
        public bool IsEditable { get; set; }

        public TripDriversVM()
        {
            Drivers = new List<DriverInfoVM>();
            SelectedDrivers = new List<DriverInfoVM>();
        }
    }
}
