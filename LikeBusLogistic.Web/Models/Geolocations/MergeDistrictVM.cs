using LikeBusLogistic.VM.ViewModels;
using System.Collections.Generic;

namespace LikeBusLogistic.Web.Models.Geolocations
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
