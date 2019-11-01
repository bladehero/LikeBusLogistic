namespace LikeBusLogistic.DAL.Models
{
    public interface IUserTrackedEntity
    {
        int? CreatedBy { get; set; }
        int? ModifiedBy { get; set; }
    }
}
