using System;
namespace LikeBusLogistic.DAL.Models
{
    public class Trip : UserTrackedEntity
    {
        public int BusId { get; set; }
        public int ScheduleId { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
    }
}