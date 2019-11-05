using LikeBusLogistic.DAL.Models;
using System.Data;
namespace LikeBusLogistic.DAL.Dao
{
    public class RouteDao : BaseDao<Route>
    {
        public RouteDao(IDbConnection connection) : base("dbo.Route", connection) { }
    }
}