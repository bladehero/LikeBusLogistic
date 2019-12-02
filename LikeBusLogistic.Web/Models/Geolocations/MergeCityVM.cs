using LikeBusLogistic.VM.ViewModels;
using System.Collections.Generic;

namespace LikeBusLogistic.Web.Models.Geolocations
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
