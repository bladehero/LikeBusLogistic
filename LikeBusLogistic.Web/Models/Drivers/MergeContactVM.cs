using LikeBusLogistic.VM.ViewModels;
using System.Collections.Generic;

namespace LikeBusLogistic.Web.Models.Drivers
{
    public class MergeContactVM
    {
        public DriverContactVM Contact { get; set; }
        public IEnumerable<DriverInfoVM> Drivers { get; set; }

        public MergeContactVM()
        {
            Drivers = new List<DriverInfoVM>();
        }
    }
}
