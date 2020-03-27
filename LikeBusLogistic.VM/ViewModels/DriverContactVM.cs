using System.Linq;
using System.Text.RegularExpressions;

namespace LikeBusLogistic.VM.ViewModels
{
    public class DriverContactVM
    {
        public int Id { get; set; }
        public int DriverId { get; set; }
        public string DriverInfo { get; set; }
        public string Contact { get; set; }
        public bool IsDeleted { get; set; }

        public string FullNameWithInitials => Regex.Replace(DriverInfo ?? "", @"(\w+)\s(\w+)\s(\w+)", m => string.Format("{0} {1}. {2}.", m.Groups[1], m.Groups[2].Value.FirstOrDefault(), m.Groups[3].Value.FirstOrDefault()), RegexOptions.IgnoreCase);
    }
}
