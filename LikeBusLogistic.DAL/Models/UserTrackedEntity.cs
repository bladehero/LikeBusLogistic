using LikeBusLogistic.DAL.Attributes;

namespace LikeBusLogistic.DAL.Models
{
    public abstract class UserTrackedEntity : BaseEntity
    {
        [Ignore]
        public int? CreatedBy { get; set; }
        [Ignore]
        public int? ModifiedBy { get; set; }
    }
}
