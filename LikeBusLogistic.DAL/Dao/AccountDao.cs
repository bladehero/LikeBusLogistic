using LikeBusLogistic.DAL.Models;
using System.Data;
namespace LikeBusLogistic.DAL.Dao
{
    public class AccountDao : BaseDao<Account>
    {
        public AccountDao(IDbConnection connection) : base("dbo.Account", connection) { }
    }
}