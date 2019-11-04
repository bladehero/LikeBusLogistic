namespace LikeBusLogistic.DAL.Models
{
    public class City : UserTrackedEntity
    {
        public string Name { get; set; }
        public int DistrictId { get; set; }
    }
}