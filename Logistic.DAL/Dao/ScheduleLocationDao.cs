using Dapper;
using Logistic.DAL.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Logistic.DAL.Dao
{
    public class ScheduleLocationDao : BaseDao<ScheduleLocation>
    {
        public ScheduleLocationDao(IDbConnection connection) : base("dbo.ScheduleRouteLocation", connection) { }

        public bool MergeScheduleRouteLocations(int scheduleId, IEnumerable<ScheduleLocation> scheduleRouteLocations)
        {
            var sb = new StringBuilder(
            "merge dbo.ScheduleLocation as trg " +
            "using" +
            "(" +
            "  select s.ScheduleId" +
            "       , s.PreviousLocationId" +
            "       , s.CurrentLocationId" +
            "       , s.ArrivalTime" +
            "       , s.DepartureTime " +
            "       , s.Distance " +
            "       , s.ModifiedBy " +
            "  from" +
            "  (" +
            "    values");

            var first = scheduleRouteLocations.First();
            //var last = scheduleRouteLocations.Last();
            foreach (var item in scheduleRouteLocations)
            {
                if (!first.Equals(item))
                {
                    sb.Append(',');
                }
                sb.Append('(');
                sb.Append(scheduleId);
                sb.Append(',');
                sb.Append(item.PreviousLocationId?.ToString() ?? "null");
                sb.Append(',');
                sb.Append(item.CurrentLocationId);
                sb.Append(',');
                sb.Append(item.ArrivalTime.HasValue ? $"\'{item.ArrivalTime.Value.ToString()}\'" : "null");
                sb.Append(',');
                sb.Append(item.DepartureTime.HasValue ? $"\'{item.DepartureTime.Value.ToString()}\'" : "null");
                sb.Append(',');
                sb.Append(item.Distance);
                sb.Append(',');
                sb.Append(item.ModifiedBy);
                sb.Append(')');
            }

            sb.Append(
            "   ) as s(ScheduleId, PreviousLocationId, CurrentLocationId, ArrivalTime, DepartureTime, Distance, ModifiedBy)" +
            ") as src " +
            "on trg.ScheduleId = src.ScheduleId and trg.CurrentLocationId = src.CurrentLocationId " +
            "  when matched then " +
            "    update set ScheduleId = src.ScheduleId " +
            "             , PreviousLocationId = src.PreviousLocationId " +
            "             , CurrentLocationId = src.CurrentLocationId " +
            "             , ArrivalTime = src.ArrivalTime " +
            "             , DepartureTime = src.DepartureTime " +
            "             , ModifiedBy = src.ModifiedBy " +
            "             , Distance = src.Distance " +
            "             , DateModified = getdate() " +
            "  when not matched by target then " +
            "    insert(ScheduleId, PreviousLocationId, CurrentLocationId, ArrivalTime, DepartureTime, Distance, ModifiedBy, CreatedBy) " +
            "    values(src.ScheduleId, src.PreviousLocationId, CurrentLocationId, src.ArrivalTime, src.DepartureTime, src.Distance, src.ModifiedBy, src.ModifiedBy) " +
            $"  when not matched by source and {scheduleId} = trg.ScheduleId then " +
            "    update set IsDeleted = 1;");

            var sql = sb.ToString();
            return Connection.Execute(sql) > 0;
        }
    }
}