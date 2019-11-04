namespace LikeBusLogistic.DAL.Models
{
    public class Account : BaseEntity
    {
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}