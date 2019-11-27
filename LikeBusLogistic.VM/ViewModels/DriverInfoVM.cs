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
        public bool IsDeleted { get; set; }
    }
}
