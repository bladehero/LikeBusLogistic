using Logistic.DAL.Models;
using System.Data;
namespace Logistic.DAL.Dao
{
    public class DriverDao : BaseDao<Driver>
    {
        public DriverDao(IDbConnection connection) : base("dbo.Driver", connection) { }
    }
}