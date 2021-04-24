using Logistic.VM.ViewModels;
using System.Collections.Generic;

namespace Logistic.Web.Models.Geolocations
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
