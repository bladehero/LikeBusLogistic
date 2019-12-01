namespace LikeBusLogistic.DAL.StoredProcedureResults
{
    public class GetDistrict_Result
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
