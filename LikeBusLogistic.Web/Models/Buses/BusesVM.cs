using LikeBusLogistic.VM.ViewModels;
using System.Collections.Generic;

namespace LikeBusLogistic.Web.Models.Buses
{
    public class BusesVM
    {
        public IEnumerable<BusVM> Buses { get; set; }

        public BusesVM()
        {
            Buses = new List<BusVM>();
        }
    }
}
