using System;
namespace LikeBusLogistic.DAL.Models
{
    public class Trip : UserTrackedEntity
    {
        public int ScheduleId { get; set; }
        public DateTime Departure { get; set; }
        public string Status { get; set; }
    }
}