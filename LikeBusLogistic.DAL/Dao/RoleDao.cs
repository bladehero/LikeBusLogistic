using LikeBusLogistic.DAL.Models;
using System.Data;
namespace LikeBusLogistic.DAL.Dao
{
    public class RoleDao : BaseDao<Role>
    {
        public RoleDao(IDbConnection connection) : base("dbo.Role", connection) { }
    }
}