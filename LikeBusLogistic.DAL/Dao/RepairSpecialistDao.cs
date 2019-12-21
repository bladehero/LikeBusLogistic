using Dapper;
using LikeBusLogistic.DAL.Models;
using System.Data;
namespace LikeBusLogistic.DAL.Dao
{
    public class RepairSpecialistDao : BaseDao<RepairSpecialist>
    {
        public RepairSpecialistDao(IDbConnection connection) : base("dbo.RepairSpecialist", connection) { }

        public RepairSpecialist FindRepairSpecialistByLocationId(int? locationId, bool withDeleted = false)
        {
            return Connection.QueryFirstOrDefault<RepairSpecialist>($"select top 1 * from {TableName} where LocationId = {locationId}{(withDeleted ? string.Empty : " and IsDeleted = 0")}");
        }
    }
}