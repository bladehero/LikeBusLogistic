using Logistic.VM.ViewModels;
using System.Collections.Generic;

namespace Logistic.Web.Models.Geolocations
{
    public class MergeCityVM
    {
        public CityVM City { get; set; }
        public IEnumerable<DistrictVM> Districts { get; set; }

        public MergeCityVM()
        {
            Districts = new List<DistrictVM>();
        }
    }
}
