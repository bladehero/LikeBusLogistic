using LikeBusLogistic.DAL.Models;
using System.Data;
namespace LikeBusLogistic.DAL.Dao
{
    public class VehicleDao : BaseDao<Vehicle>
    {
        public VehicleDao(IDbConnection connection) : base("dbo.Vehicle", connection) { }
    }
}