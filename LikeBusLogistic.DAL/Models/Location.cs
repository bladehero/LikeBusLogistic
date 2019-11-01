namespace LikeBusLogistic.DAL.Models
{
    public class Location
    {
        public int? CountryId { get; set; }
        public int? DistrictId { get; set; }
        public string Name { get; set; }
        public float Latitude { get; set; }
        public float Longtitude { get; set; }
        public bool IsParking { get; set; }
        public bool IsCarRepair { get; set; }
    }
}