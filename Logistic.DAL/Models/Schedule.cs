namespace Logistic.DAL.Models
{
    public class Schedule : UserTrackedEntity
    {
        public string Name { get; set; }
        public int RouteId { get; set; }
    }
}