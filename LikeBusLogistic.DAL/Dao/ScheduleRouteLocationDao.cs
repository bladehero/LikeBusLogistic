using LikeBusLogistic.DAL.Models;
using System.Data;

namespace LikeBusLogistic.DAL.Dao
{
    public class ScheduleRouteLocationDao : BaseDao<ScheduleRouteLocation>
    {
        public ScheduleRouteLocationDao(IDbConnection connection) : base("dbo.ScheduleRouteLocation", connection) { }
    }
}