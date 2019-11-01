namespace LikeBusLogistic.DAL.Models
{
    public class Route
    {
        public int? DepartureId { get; set; }
        public int? ArrivalId { get; set; }
        public string Name { get; set; }
        public float EstimatedDurationInHours { get; set; }
    }
}