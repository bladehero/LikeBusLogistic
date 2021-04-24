using Logistic.DAL.Models;
using System.Data;
namespace Logistic.DAL.Dao
{
    public class RouteLocationDao : BaseDao<RouteLocation>
    {
        public RouteLocationDao(IDbConnection connection) : base("dbo.RouteLocation", connection) { }
    }
}