using LikeBusLogistic.DAL.Models;
using System.Data;
namespace LikeBusLogistic.DAL.Dao
{
    public class TripBusDriverDao : BaseDao<TripBusDriver>
    {
        public TripBusDriverDao(IDbConnection connection) : base("dbo.TripBusDriver", connection) { }
    }
}