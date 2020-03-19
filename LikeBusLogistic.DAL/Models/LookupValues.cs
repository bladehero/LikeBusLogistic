namespace LikeBusLogistic.DAL.Models
{
    public class LookupValues : UserTrackedEntity
    {
        public int LookupId { get; set; }
        public string Value { get; set; }
        public string DisplayText { get; set; }
        public bool? IsEditable { get; set; }
        public int? SortOrder { get; set; }
    }
}