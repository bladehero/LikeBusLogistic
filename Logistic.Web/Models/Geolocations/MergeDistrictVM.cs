using Logistic.VM.ViewModels;
using System.Collections.Generic;

namespace Logistic.Web.Models.Geolocations
{
    public class MergeDistrictVM
    {
        public DistrictVM District { get; set; }
        public IEnumerable<CountryVM> Countries { get; set; }

        public MergeDistrictVM()
        {
            Countries = new List<CountryVM>();
        }
    }
}
