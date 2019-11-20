using LikeBusLogistic.VM.ViewModels;
using System.Collections.Generic;

namespace LikeBusLogistic.Web.Models.Drivers
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
