using LikeBusLogistic.DAL.Models;
using System.Data;
namespace LikeBusLogistic.DAL.Dao
{
    public class BusDriverDao : BaseDao<BusDriver>
    {
        public BusDriverDao(IDbConnection connection) : base("dbo.BusDriver", connection) { }
    }
}