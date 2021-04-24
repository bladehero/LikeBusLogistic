using Logistic.VM.ViewModels;
using System.Collections.Generic;

namespace Logistic.Web.Models.Geolocations
{
    public class FullInformationVM
    {
        public IEnumerable<CountryVM> Countries { get; set; }
        public IEnumerable<DistrictVM> Districts { get; set; }
        public IEnumerable<CityVM> Cities { get; set; }
        public IEnumerable<VM.ViewModels.LocationVM> Locations { get; set; }
        public GeolocationTab Tab { get; set; }

        public FullInformationVM()
        {
            Countries = new List<CountryVM>();
            Districts = new List<DistrictVM>();
            Cities = new List<CityVM>();
            Locations = new List<VM.ViewModels.LocationVM>();
        }
    }
}
