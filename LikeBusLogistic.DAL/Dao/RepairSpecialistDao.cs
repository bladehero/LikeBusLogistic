using Dapper;
using LikeBusLogistic.DAL.Models;
using System.Collections.Generic;
using System.Data;
namespace LikeBusLogistic.DAL.Dao
{
    public class RepairSpecialistDao : BaseDao<RepairSpecialist>
    {
        public RepairSpecialistDao(IDbConnection connection) : base("dbo.RepairSpecialist", connection) { }

        public IEnumerable<RepairSpecialist> FindRepairSpecialistsByLocationId(int? locationId, bool withDeleted = false)
        {
            return Connection.Query<RepairSpecialist>($"select * from {TableName} where LocationId = {locationId}{(withDeleted ? string.Empty : " and IsDeleted = 0")}");
        }
    }
}