namespace LikeBusLogistic.DAL.Models
{
    public class RouteLocation : BaseEntity
    {
        public int RouteId { get; set; }
        public int CurrentLocationId { get; set; }
        public int PreviousLocationId { get; set; }
        public float? StopDurationInHours { get; set; }
        public float EstimatedDurationInHours { get; set; }
    }
}