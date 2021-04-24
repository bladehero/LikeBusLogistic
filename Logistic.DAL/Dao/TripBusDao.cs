using Logistic.DAL.Models;
using System.Data;
namespace Logistic.DAL.Dao
{
    public class TripBusDriverDao : BaseDao<TripBusDriver>
    {
        public TripBusDriverDao(IDbConnection connection) : base("dbo.TripBusDriver", connection) { }
    }
}