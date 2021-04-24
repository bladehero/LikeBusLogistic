using Logistic.DAL.Models;
using System.Data;
namespace Logistic.DAL.Dao
{
    public class UserDao : BaseDao<User>
    {
        public UserDao(IDbConnection connection) : base("dbo.User", connection) { }
    }
}
