using Dapper;
using LikeBusLogistic.DAL.Models;
using System.Collections.Generic;
using System.Data;
namespace LikeBusLogistic.DAL.Dao
{
    public class ScheduleDao : BaseDao<Schedule>
    {
        public ScheduleDao(IDbConnection connection) : base("dbo.Schedule", connection) { }

        public IEnumerable<Schedule> UpdateNeedsSyncByRouteId(int? routeId)
        {
            Connection.Execute($"update {TableName} set NeedsSync = 1 where RouteId = {routeId?.ToString() ?? "null"}");
            return Connection.Query<Schedule>($"{SelectFromString} where RouteId = {routeId?.ToString() ?? "null"}");
        }
    }
}