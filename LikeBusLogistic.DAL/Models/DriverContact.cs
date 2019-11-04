namespace LikeBusLogistic.DAL.Models
{
    public class DriverContact : UserTrackedEntity
    {
        public int DriverId { get; set; }
        public string Contact { get; set; }
    }
}