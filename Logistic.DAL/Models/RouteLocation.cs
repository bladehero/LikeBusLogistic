namespace Logistic.DAL.Models
{
    public class RouteLocation : UserTrackedEntity
    {
        public int RouteId { get; set; }
        public int CurrentLocationId { get; set; }
        public int? PreviousLocationId { get; set; }
        public double Distance { get; set; }
    }
}