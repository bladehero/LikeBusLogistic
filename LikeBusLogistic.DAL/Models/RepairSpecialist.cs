namespace LikeBusLogistic.DAL.Models
{
    public class RepairSpecialist : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int LocationId { get; set; }
        public string Contact { get; set; }
    }
}