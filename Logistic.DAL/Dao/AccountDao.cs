using Dapper;
using Logistic.DAL.Models;
using System.Data;
namespace Logistic.DAL.Dao
{
    public class AccountDao : BaseDao<Account>
    {
        public AccountDao(IDbConnection connection) : base("dbo.Account", connection) { }

        public override int Insert(Account item)
        {
            item.Password = MD5HashPassword(item.Password);
            return base.Insert(item);
        }
        public override bool Update(Account item)
        {
            item.Password = MD5HashPassword(item.Password);
            return base.Update(item);
        }

        private string MD5HashPassword(string password)
        {
            var parameters = new
            {
                @password = password
            };

            return Connection.ExecuteScalar<string>("dbo.MD5HashPassword", parameters, commandType: CommandType.StoredProcedure);
        }
    }
}