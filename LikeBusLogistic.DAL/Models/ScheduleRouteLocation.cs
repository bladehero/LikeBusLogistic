﻿using System;
namespace LikeBusLogistic.DAL.Models
{
    public class ScheduleRouteLocation : BaseEntity
    {
        public string Name { get; set; }
        public int ScheduleId { get; set; }
        public int RouteLocationId { get; set; }
        public TimeSpan? ArrivalTime { get; set; }
        public TimeSpan? DeparuteTime { get; set; }
    }
}