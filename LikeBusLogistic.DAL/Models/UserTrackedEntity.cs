using LikeBusLogistic.DAL.Attributes;

namespace LikeBusLogistic.DAL.Models
{
    public abstract class UserTrackedEntity : BaseEntity
    {
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
