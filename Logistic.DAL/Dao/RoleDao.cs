using Logistic.DAL.Models;
using System.Data;
namespace Logistic.DAL.Dao
{
    public class RoleDao : BaseDao<Role>
    {
        public RoleDao(IDbConnection connection) : base("dbo.Role", connection) { }
    }
}