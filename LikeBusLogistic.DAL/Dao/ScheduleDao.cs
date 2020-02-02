using LikeBusLogistic.DAL.Models;
using System.Data;
namespace LikeBusLogistic.DAL.Dao
{
    public class ScheduleDao : BaseDao<Schedule>
    {
        public ScheduleDao(IDbConnection connection) : base("dbo.Schedule", connection) { }
    }
}