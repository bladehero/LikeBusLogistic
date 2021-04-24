using Logistic.DAL.Models;
using System.Data;
namespace Logistic.DAL.Dao
{
    public class TripBusDao : BaseDao<TripBus>
    {
        public TripBusDao(IDbConnection connection) : base("dbo.TripBus", connection) { }
    }
}