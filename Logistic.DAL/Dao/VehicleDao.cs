using Logistic.DAL.Models;
using System.Data;
namespace Logistic.DAL.Dao
{
    public class VehicleDao : BaseDao<Vehicle>
    {
        public VehicleDao(IDbConnection connection) : base("dbo.Vehicle", connection) { }
    }
}