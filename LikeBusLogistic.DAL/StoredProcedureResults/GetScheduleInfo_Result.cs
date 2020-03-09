﻿using System;

namespace LikeBusLogistic.DAL.StoredProcedureResults
{
    public class GetScheduleInfo_Result
    {
        public int ScheduleId { get; set; }
        public string ScheduleName { get; set; }
        public int RouteId { get; set; }
        public string RouteName { get; set; }
        public int ScheduleRouteLocationId { get; set; }
        public string ScheduleRouteLocationName { get; set; }
        public TimeSpan? ArrivalTime { get; set; }
        public TimeSpan? DeparuteTime { get; set; }
        public string LocationFullName { get; set; }
        public string LocationLatitude { get; set; }
        public string LocationLongtitude { get; set; }
        public string LocationIsCarRepair { get; set; }
        public string LocationIsParking { get; set; }
        public string LocationCityId { get; set; }
        public string LocationCityName { get; set; }
        public string LocationDistrictId { get; set; }
        public string LocationDistrictName { get; set; }
        public string LocationCountryId { get; set; }
        public string LocationCountryName { get; set; }
        public string IsDeleted { get; set; }
    }
}
