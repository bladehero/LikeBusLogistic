﻿using Dapper;
using LikeBusLogistic.DAL.StoredProcedureResults;
using System.Collections.Generic;
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
        public IEnumerable<GetUserAccountByCredentials_Result> GetUserAccountById(int id)
        {
            var parameters = new
            {
                @id = id
            };

            return Connection.Query<GetUserAccountByCredentials_Result>("dbo.GetUserAccountById", parameters, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<GetUserAccountByCredentials_Result> GetUserAccountByCredentials(string login, string password)
        {
            var parameters = new
            {
                @login = login,
                @password = password
            };

            return Connection.Query<GetUserAccountByCredentials_Result>("dbo.GetUserAccountByCredentials", parameters, commandType: CommandType.StoredProcedure);
        }
    }
}
