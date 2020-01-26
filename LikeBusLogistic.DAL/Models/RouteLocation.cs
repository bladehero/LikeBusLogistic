namespace LikeBusLogistic.DAL.Models
{
    public class RouteLocation : UserTrackedEntity
    {
        public int RouteId { get; set; }
        public int CurrentLocationId { get; set; }
        public int? PreviousLocationId { get; set; }
    }
}