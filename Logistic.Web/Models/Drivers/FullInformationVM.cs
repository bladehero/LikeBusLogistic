using Logistic.VM.ViewModels;
using System.Collections.Generic;

namespace Logistic.Web.Models.Drivers
{
    public class FullInformationVM
    {
        public IEnumerable<DriverInfoVM> Drivers{ get; set; }
        public IEnumerable<DriverContactVM> Contacts { get; set; }
        public DriverTab Tab { get; set; }

        public FullInformationVM()
        {
            Drivers = new List<DriverInfoVM>();
            Contacts = new List<DriverContactVM>();
        }
    }
}
