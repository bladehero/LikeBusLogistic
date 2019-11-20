using LikeBusLogistic.DAL.Models;
using System.Collections.Generic;
using System.Data;
namespace LikeBusLogistic.DAL.Dao
{
    public class BusDriverDao : BaseDao<BusDriver>
    {
        public BusDriverDao(IDbConnection connection) : base("dbo.BusDriver", connection) { }

        public IEnumerable<BusDriver> GetBusDriver(int driverId)
        {
            return Query($"{SelectFromString} where DriverId = {driverId}");
        }
        public BusDriver GetBusDriver(int busId, int driverId)
        {
            return QueryFirstOrDefault($"{SelectFromString} where BusId = {busId} and DriverId = {driverId}");
        }
    }
}