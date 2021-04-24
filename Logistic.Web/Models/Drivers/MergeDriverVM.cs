using Logistic.VM.ViewModels;
using System.Collections.Generic;

namespace Logistic.Web.Models.Drivers
{
    public class MergeDriverVM
    {
        public DriverInfoVM Driver { get; set; }
        public IEnumerable<BusVM> Buses { get; set; }

        public MergeDriverVM()
        {
            Buses = new List<BusVM>();
        }
    }
}
