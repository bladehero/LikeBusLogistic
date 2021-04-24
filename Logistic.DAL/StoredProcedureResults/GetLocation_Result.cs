﻿namespace Logistic.DAL.StoredProcedureResults
{
    public class GetLocation_Result
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsParking { get; set; }
        public bool IsCarRepair { get; set; }
        public int? CityId { get; set; }
        public string CityName { get; set; }
        public int? DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
