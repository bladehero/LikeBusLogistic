using LikeBusLogistic.DAL.Models;
using System.Data;
namespace LikeBusLogistic.DAL.Dao
{
    public class RouteLocationDao : BaseDao<RouteLocation>
    {
        public RouteLocationDao(IDbConnection connection) : base("dbo.RouteLocation", connection) { }
    }
}