namespace LikeBusLogistic.DAL.Models
{
    public class Vehicle : UserTrackedEntity
    {
        public string Producer { get; set; }
        public string Model { get; set; }
        public int PassengerCapacity { get; set; }
        public string Description { get; set; }
    }
}