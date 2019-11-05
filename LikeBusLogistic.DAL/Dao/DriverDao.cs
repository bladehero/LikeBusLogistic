using LikeBusLogistic.DAL.Models;
using System.Data;
namespace LikeBusLogistic.DAL.Dao
{
    public class DriverDao : BaseDao<Driver>
    {
        public DriverDao(IDbConnection connection) : base("dbo.Driver", connection) { }
    }
}