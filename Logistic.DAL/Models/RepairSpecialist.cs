namespace Logistic.DAL.Models
{
    public class RepairSpecialist : UserTrackedEntity
    {
        public string Name { get; set; }
        public int LocationId { get; set; }
        public string Contact { get; set; }
    }
}