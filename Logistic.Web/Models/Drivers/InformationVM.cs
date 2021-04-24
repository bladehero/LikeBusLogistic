using Logistic.VM.ViewModels;
using System.Collections.Generic;

namespace Logistic.Web.Models.Drivers
{
    public class InformationVM
    {
        public IEnumerable<DriverInfoVM> Drivers { get; set; }

        public InformationVM()
        {
            Drivers = new List<DriverInfoVM>();
        }
    }
}
