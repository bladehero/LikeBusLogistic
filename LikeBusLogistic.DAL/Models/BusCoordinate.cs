namespace LikeBusLogistic.DAL.Models
{
    public class BusCoordinate : UserTrackedEntity
    {
        public int BusId { get; set; }
        public int? LocationId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
