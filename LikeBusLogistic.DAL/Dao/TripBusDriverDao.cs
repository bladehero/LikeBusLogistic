using LikeBusLogistic.DAL.Models;
using System.Data;
namespace LikeBusLogistic.DAL.Dao
{
    public class TripBusDao : BaseDao<TripBus>
    {
        public TripBusDao(IDbConnection connection) : base("dbo.TripBus", connection) { }
    }
}