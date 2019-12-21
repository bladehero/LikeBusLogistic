using LikeBusLogistic.VM.ViewModels;
using System.Collections.Generic;

namespace LikeBusLogistic.Web.Models.Geolocations
{
    public class RepairSpecialistsForLocationVM
    {
        public VM.ViewModels.LocationVM Location { get; set; }
        public IEnumerable<RepairSpecialistVM> RepairSpecialists { get; set; }

        public RepairSpecialistsForLocationVM()
        {
            RepairSpecialists = new List<RepairSpecialistVM>();
        }
    }
}
