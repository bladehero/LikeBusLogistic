namespace LikeBusLogistic.VM.ViewModels
{
    public class RepairSpecialistVM
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int LocationId { get; set; }
        public string Contact { get; set; }
        public bool IsDeleted { get; set; }
    }
}
