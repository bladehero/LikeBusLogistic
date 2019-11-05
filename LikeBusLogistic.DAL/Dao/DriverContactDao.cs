using LikeBusLogistic.DAL.Models;
using System.Data;
namespace LikeBusLogistic.DAL.Dao
{
    public class DriverContactDao : BaseDao<DriverContact>
    {
        public DriverContactDao(IDbConnection connection) : base("dbo.DriverContact", connection) { }
    }
}