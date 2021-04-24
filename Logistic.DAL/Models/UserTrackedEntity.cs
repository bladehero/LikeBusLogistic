namespace Logistic.DAL.Models
{
    public abstract class UserTrackedEntity : BaseEntity
    {
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
