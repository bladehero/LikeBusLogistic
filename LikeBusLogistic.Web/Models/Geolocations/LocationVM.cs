using LikeBusLogistic.VM.ViewModels;
using System.Collections.Generic;

namespace LikeBusLogistic.Web.Models.Geolocations
{
    public class LocationVM
    {
        public VM.ViewModels.LocationVM Location { get; set; }

        public IEnumerable<CountryVM> Countries { get; set; }
        public IEnumerable<DistrictVM> Districts { get; set; }
        public IEnumerable<CityVM> Cities { get; set; }

        public LocationVM()
        {
            Countries = new List<CountryVM>();
            Districts = new List<DistrictVM>();
            Cities = new List<CityVM>();
        }
    }
}
