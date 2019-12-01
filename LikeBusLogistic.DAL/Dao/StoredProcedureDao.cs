using Dapper;
using LikeBusLogistic.DAL.StoredProcedureResults;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LikeBusLogistic.DAL.Dao
{
    public class StoredProcedureDao : DataAccessObject
    {
        public StoredProcedureDao(IDbConnection connection) : base(connection) { }

        public GetUserAccountByCredentials_Result GetUserAccountById(int id)
        {
            var parameters = new
            {
                @id = id
            };

            return Connection.Query<GetUserAccountByCredentials_Result>("dbo.GetUserAccountById", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
        }
        public GetUserAccountByCredentials_Result GetUserAccountByCredentials(string login, string password)
        {
            var parameters = new
            {
                @login = login,
                @password = password
            };

            return Connection.Query<GetUserAccountByCredentials_Result>("dbo.GetUserAccountByCredentials", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
        }
        public IEnumerable<GetDriverInfo_Result> GetDriverInfo(int? driverId = null, bool withDeleted = false)
        {
            var parameters = new
            {
                @driverId = driverId,
                @withDeleted = withDeleted
            };

            return Connection.Query<GetDriverInfo_Result>("dbo.GetDriverInfo", parameters, commandType: CommandType.StoredProcedure);
        }
        public void MergeDriver(int? driverId, int busId, string firstName, string lastName, string middleName)
        {
            var parameters = new
            {
                @driverId = driverId,
                @busId = busId,
                @firstName = firstName,
                @lastName = lastName,
                @middleName = middleName,
            };

            Connection.Query<GetDriverInfo_Result>("dbo.MergeDriver", parameters, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<GetCity_Result> GetCity(int? cityId = null, bool withDeleted = false)
        {
            var parameters = new
            {
                @cityId = cityId,
                @withDeleted = withDeleted
            };

            return Connection.Query<GetCity_Result>("dbo.GetCity", parameters, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<GetDistrict_Result> GetDistrict(int? districtId = null, bool withDeleted = false)
        {
            var parameters = new
            {
                @districtId = districtId,
                @withDeleted = withDeleted
            };

            return Connection.Query<GetDistrict_Result>("dbo.GetDistrict", parameters, commandType: CommandType.StoredProcedure);
        }
        public IEnumerable<GetLocation_Result> GetLocation(int? locationId = null, bool withDeleted = false)
        {
            var parameters = new
            {
                @locationId = locationId,
                @withDeleted = withDeleted
            };

            return Connection.Query<GetLocation_Result>("dbo.GetLocation", parameters, commandType: CommandType.StoredProcedure);
        }
    }
}
