namespace LikeBusLogistic.DAL.Models
{
    public class TripDriver : UserTrackedEntity
    {
        public int DriverId { get; set; }
        public int TripId { get; set; }
    }
}