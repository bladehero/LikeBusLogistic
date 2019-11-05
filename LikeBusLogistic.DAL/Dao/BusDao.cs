using LikeBusLogistic.DAL.Models;
using System.Data;
namespace LikeBusLogistic.DAL.Dao
{
    public class BusDao : BaseDao<Bus>
    {
        public BusDao(IDbConnection connection) : base("dbo.Bus", connection) { }
    }
}