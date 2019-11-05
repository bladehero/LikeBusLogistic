namespace LikeBusLogistic.DAL.Models
{
    public class TripBusDriver : BaseEntity
    {
        public int? BusId { get; set; }
        public int? DriverId { get; set; }
        public int TripId { get; set; }
        public int LocationId { get; set; }
    }
}