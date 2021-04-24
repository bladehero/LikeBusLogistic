using Logistic.VM.ViewModels;
using System.Collections.Generic;

namespace Logistic.Web.Models.Drivers
{
    public class ContactsVM
    {
        public IEnumerable<DriverContactVM> Contacts { get; set; }

        public ContactsVM()
        {
            Contacts = new List<DriverContactVM>();
        }
    }
}
