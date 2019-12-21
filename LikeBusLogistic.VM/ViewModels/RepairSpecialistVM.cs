namespace LikeBusLogistic.VM.ViewModels
{
    public class RepairSpecialistVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LocationId { get; set; }
        public string Contact { get; set; }
        public bool IsDeleted { get; set; }
    }
}
