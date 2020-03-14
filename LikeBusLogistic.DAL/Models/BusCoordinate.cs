namespace LikeBusLogistic.DAL.Models
{
    public class BusCoordinate
    {
        public int BusId { get; set; }
        public int? LocationId { get; set; }
        public double? Latitude { get; set; }
        public double? Longtitude { get; set; }
    }
}
