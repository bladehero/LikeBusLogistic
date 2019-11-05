using Dapper;
using System.Data;

namespace LikeBusLogistic.DAL.Dao
{
    public class StoredProcedureDao : DataAccessObject
    {
        public StoredProcedureDao(IDbConnection connection) : base(connection) { }

        public string MD5HashPassword(string password)
        {
            var parameters = new
            {
                @password = password
            };

            return Connection.ExecuteScalar<string>("dbo.MD5HashPassword", parameters, commandType: CommandType.StoredProcedure);
        }
        public string GetUserAccount(string login, string password)
        {
            var parameters = new
            {
                @login = login,
                @password = password
            };

            return Connection.ExecuteScalar<string>("dbo.GetUserAccount", parameters, commandType: CommandType.StoredProcedure);
        }
    }
}
