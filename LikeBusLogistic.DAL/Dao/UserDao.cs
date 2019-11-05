using LikeBusLogistic.DAL.Models;
using System.Data;
namespace LikeBusLogistic.DAL.Dao
{
    public class UserDao : BaseDao<User>
    {
        public UserDao(IDbConnection connection) : base("dbo.User", connection) { }
    }
}
