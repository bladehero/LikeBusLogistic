using System;
namespace LikeBusLogistic.DAL.Models
{
    public class ScheduleLocation : UserTrackedEntity
    {
        public int ScheduleId { get; set; }
        public int? PreviousLocationId { get; set; }
        public int CurrentLocationId { get; set; }
        public double Distance { get; set; }
        public TimeSpan? ArrivalTime { get; set; }
        public TimeSpan? DepartureTime { get; set; }
    }
}