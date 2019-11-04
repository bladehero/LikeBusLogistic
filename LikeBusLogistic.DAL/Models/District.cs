namespace LikeBusLogistic.DAL.Models
{
    public class District : UserTrackedEntity
    {
        public string Name { get; set; }
        public int CountryId { get; set; }
    }
}