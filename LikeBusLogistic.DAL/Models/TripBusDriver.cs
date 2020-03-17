namespace LikeBusLogistic.DAL.Models
{
    public class TripBusDriver : UserTrackedEntity
    {
        public int DriverId { get; set; }
        public int TripBusId { get; set; }
    }
}