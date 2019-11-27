namespace LikeBusLogistic.VM.ViewModels
{
    public class DriverContactVM
    {
        public int Id { get; set; }
        public int DriverId { get; set; }
        public string DriverInfo { get; set; }
        public string Contact { get; set; }
        public bool IsDeleted { get; set; }
    }
}
