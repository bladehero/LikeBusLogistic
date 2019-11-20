using LikeBusLogistic.VM.ViewModels;
using System.Collections.Generic;

namespace LikeBusLogistic.Web.Models.Drivers
{
    public class FullInformationVM
    {
        public IEnumerable<DriverInfoVM> Drivers{ get; set; }
        public IEnumerable<DriverContactVM> Contacts { get; set; }

        public FullInformationVM()
        {
            Drivers = new List<DriverInfoVM>();
            Contacts = new List<DriverContactVM>();
        }
    }
}
