using System.Linq;
using System.Text.RegularExpressions;

namespace LikeBusLogistic.VM.ViewModels
{
    public class DriverInfoVM
    {
        public int BusId { get; set; }
        public int? DriverId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string BusInfo { get; set; }
        public bool? AttachedOnBus { get; set; }
        public bool IsDeleted { get; set; }

        public string FullNameWithInitials => Regex.Replace($"{LastName} {FirstName} {MiddleName}", @"(\w+)\s(\w+)\s(\w+)", m => string.Format("{0} {1}. {2}.", m.Groups[1], m.Groups[2].Value.FirstOrDefault(), m.Groups[3].Value.FirstOrDefault()), RegexOptions.IgnoreCase);
    }
}
