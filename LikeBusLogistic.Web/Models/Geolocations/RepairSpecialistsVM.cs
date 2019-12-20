using LikeBusLogistic.VM.ViewModels;
using System.Collections.Generic;

namespace LikeBusLogistic.Web.Models.Geolocations
{
    public class RepairSpecialistsVM
    {
        public LocationVM Location { get; set; }
        public IEnumerable<RepairSpecialistVM> RepairSpecialists { get; set; }

        public RepairSpecialistsVM()
        {
            RepairSpecialists = new List<RepairSpecialistVM>();
        }
    }
}
