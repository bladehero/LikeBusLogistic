namespace LikeBusLogistic.VM.ViewModels
{
    public class RouteVM
    {
        public int Id { get; set; }
        public int? DepartureId { get; set; }
        public int? ArrivalId { get; set; }
        public string Name { get; set; }
        public float EstimatedDurationInHours { get; set; }
        public bool IsDeleted { get; set; }
    }
}
