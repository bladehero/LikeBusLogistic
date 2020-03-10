using System;

namespace LikeBusLogistic.VM.ViewModels
{
    public class ScheduleRouteLocationVM
    {
        public int ScheduleId { get; set; }
        public int RouteLocationId { get; set; }
        public TimeSpan? ArrivalTime { get; set; }
        public TimeSpan? DeparuteTime { get; set; }

        public string ScheduleName { get; set; }
        public double Distance { get; set; }

        public string LocationName { get; set; }
        public string CityName { get; set; }
        public string DistrictName { get; set; }
        public string CountryName { get; set; }
        public string RouteLocationFullName =>
            string.Join(", ", new string[] { LocationName, CityName, DistrictName, CountryName });

        public int? DurationInMinutes => 
            DeparuteTime.HasValue && ArrivalTime.HasValue ?
            Math.Abs((int)(ArrivalTime.Value.TotalMinutes - DeparuteTime.Value.TotalMinutes))
            : 0;

        public bool IsDeleted { get; set; }
    }
}
