using Dapper;
using LikeBusLogistic.DAL.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LikeBusLogistic.DAL.Dao
{
    public class ScheduleRouteLocationDao : BaseDao<ScheduleRouteLocation>
    {
        public ScheduleRouteLocationDao(IDbConnection connection) : base("dbo.ScheduleRouteLocation", connection) { }

        public bool MergeScheduleRouteLocations(IEnumerable<ScheduleRouteLocation> scheduleRouteLocations)
        {
            var sb = new StringBuilder(
            "merge dbo.ScheduleRouteLocation as trg " +
            "using" +
            "(" +
            "  select s.ScheduleId" +
            "       , s.RouteLocationId" +
            "       , s.ArrivalTime" +
            "       , s.DepartureTime " +
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
                sb.Append(item.ScheduleId);
                sb.Append(',');
                sb.Append(item.RouteLocationId);
                sb.Append(',');
                if (item.ArrivalTime.HasValue)
                {
                    sb.Append('\'');
                    sb.Append(item.ArrivalTime);
                    sb.Append('\'');
                }
                else
                {
                    sb.Append("null");
                }
                sb.Append(',');
                if (item.DepartureTime.HasValue)
                {
                    sb.Append('\'');
                    sb.Append(item.DepartureTime);
                    sb.Append('\'');
                }
                else
                {
                    sb.Append("null");
                }
                sb.Append(',');
                sb.Append(item.ModifiedBy);
                sb.Append(')');
            }

            sb.Append(
            "   ) as s(ScheduleId, RouteLocationId, ArrivalTime, DepartureTime, ModifiedBy)" +
            ") as src " +
            "on trg.ScheduleId = src.ScheduleId and trg.RouteLocationId = src.RouteLocationId " +
            "  when matched then " +
            "    update set ScheduleId = src.ScheduleId " +
            "             , RouteLocationId = src.RouteLocationId " +
            "             , ArrivalTime = src.ArrivalTime " +
            "             , DepartureTime = src.DepartureTime " +
            "             , ModifiedBy = src.ModifiedBy " +
            "             , DateModified = getdate() " +
            "  when not matched by target then " +
            "    insert(ScheduleId, RouteLocationId, ArrivalTime, DepartureTime, ModifiedBy, CreatedBy) " +
            "    values(src.ScheduleId, src.RouteLocationId, src.ArrivalTime, src.DepartureTime, src.ModifiedBy, src.ModifiedBy) " +
            "  when not matched by source then " +
            "    update set IsDeleted = 0;");

            var sql = sb.ToString();
            return Connection.Execute(sql) > 0;
        }
    }
}