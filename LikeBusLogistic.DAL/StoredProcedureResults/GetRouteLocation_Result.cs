namespace LikeBusLogistic.DAL.StoredProcedureResults
{
    public class GetRouteLocation_Result
    {
        public int RouteId { get; set; }
        public int RouteLocationId { get; set; }
        public string RouteName { get; set; }
        public double Distance { get; set; }
        public double EstimatedDurationInHours { get; set; }
        public double StopDurationInHours { get; set; }

        public int? CurrentLocationId { get; set; }
        public string CurrentFullName { get; set; }
        public string CurrentName { get; set; }
        public int? CurrentCityId { get; set; }
        public string CurrentCityName { get; set; }
        public int? CurrentCountryId { get; set; }
        public string CurrentCountryName { get; set; }
        public int? CurrentDistrictId { get; set; }
        public string CurrentDistrictName { get; set; }
        public bool? CurrentIsCarRepair { get; set; }
        public bool? CurrentIsParking { get; set; }
        public double? CurrentLatitude { get; set; }
        public double? CurrentLongitude { get; set; }

        public int? PreviousLocationId { get; set; }
        public string PreviousFullName { get; set; }
        public string PreviousName { get; set; }
        public int? PreviousCityId { get; set; }
        public string PreviousCityName { get; set; }
        public int? PreviousCountryId { get; set; }
        public string PreviousCountryName { get; set; }
        public int? PreviousDistrictId { get; set; }
        public string PreviousDistrictName { get; set; }
        public bool? PreviousIsCarRepair { get; set; }
        public bool? PreviousIsParking { get; set; }
        public double? PreviousLatitude { get; set; }
        public double? PreviousLongitude { get; set; }
    }
}
