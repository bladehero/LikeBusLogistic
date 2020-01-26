namespace LikeBusLogistic.VM.ViewModels
{
    public class RouteVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ArrivalId { get; set; }
        public string ArrivalLocationName { get; set; }
        public int? DepartureId { get; set; }
        public string DepartureLocationName { get; set; }
        public float? EstimatedDurationInHours { get; set; }
        public bool IsDeleted { get; set; }
    }
}
