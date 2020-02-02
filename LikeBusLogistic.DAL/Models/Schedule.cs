namespace LikeBusLogistic.DAL.Models
{
    public class Schedule : BaseEntity
    {
        public string Name { get; set; }
        public int RouteId { get; set; }
    }
}