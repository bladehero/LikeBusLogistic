using Logistic.DAL.Models;
using System.Data;
namespace Logistic.DAL.Dao
{
    public class LocationDao : BaseDao<Location>
    {
        public LocationDao(IDbConnection connection) : base("dbo.Location", connection) { }
    }
}