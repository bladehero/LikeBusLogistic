using System;

namespace LikeBusLogistic.VM.ViewModels
{
    public class ScheduleRouteLocationVM
    {
        public int ScheduleId { get; set; }
        public string ScheduleName { get; set; }
        public int ScheduleRouteId { get; set; }
        public int? SchedulePreviousLocationId { get; set; }
        public int ScheduleCurrentLocationId { get; set; }
        public TimeSpan? ScheduleLocationArrivalTime { get; set; }
        public TimeSpan? ScheduleLocationDepartureTime { get; set; }
        public double ScheduleLocationDistance { get; set; }
        public string ScheduleLocationCurrentFullName { get; set; }
        public string ScheduleLocationCurrentName { get; set; }
        public int? ScheduleLocationCurrentCityId { get; set; }
        public string ScheduleLocationCurrentCityName { get; set; }
        public int? ScheduleLocationCurrentCountryId { get; set; }
        public string ScheduleLocationCurrentCountryName { get; set; }
        public int? ScheduleLocationCurrentDistrictId { get; set; }
        public string ScheduleLocationCurrentDistrictName { get; set; }
        public int ScheduleLocationCurrentIsCarRepair { get; set; }
        public int ScheduleLocationCurrentIsParking { get; set; }
        public int ScheduleLocationCurrentLatitude { get; set; }
        public int ScheduleLocationCurrentLongitude { get; set; }
        public string ScheduleLocationPreviousFullName { get; set; }
        public string ScheduleLocationPreviousName { get; set; }
        public int? ScheduleLocationPreviousCityId { get; set; }
        public string ScheduleLocationPreviousCityName { get; set; }
        public int? ScheduleLocationPreviousCountryId { get; set; }
        public string ScheduleLocationPreviousCountryName { get; set; }
        public int? ScheduleLocationPreviousDistrictId { get; set; }
        public string ScheduleLocationPreviousDistrictName { get; set; }
        public bool? ScheduleLocationPreviousIsCarRepair { get; set; }
        public bool? ScheduleLocationPreviousIsParking { get; set; }
        public double? ScheduleLocationPreviousLatitude { get; set; }
        public double? ScheduleLocationPreviousLongitude { get; set; }
        public bool ScheduleLocationIsBoundary { get; set; }
    }
}
