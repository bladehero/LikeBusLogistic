namespace Logistic.DAL.Models
{
    public class TripBus : UserTrackedEntity
    {
        public int BusId { get; set; }
        public int TripId { get; set; }
        public int LocationId { get; set; }
    }
}
