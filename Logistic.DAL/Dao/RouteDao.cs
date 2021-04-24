using Dapper;
using Logistic.DAL.Models;
using System.Collections.Generic;
using System.Data;
namespace Logistic.DAL.Dao
{
    public class RouteDao : BaseDao<Route>
    {
        public RouteDao(IDbConnection connection) : base("dbo.Route", connection) { }

        public IEnumerable<Route> GetRouteByLocationCount(int count)
        {
            return Connection.Query<Route>($"{SelectFromString} as r " +
                $"where (select count(1) from RouteLocation where r.Id = RouteId) = {count}");
        }
    }
}