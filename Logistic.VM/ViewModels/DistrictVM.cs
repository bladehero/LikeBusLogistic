namespace Logistic.VM.ViewModels
{
    public class DistrictVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
