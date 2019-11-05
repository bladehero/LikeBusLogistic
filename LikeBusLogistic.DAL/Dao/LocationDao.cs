using LikeBusLogistic.DAL.Models;
using System.Data;
namespace LikeBusLogistic.DAL.Dao
{
    public class LocationDao : BaseDao<Location>
    {
        public LocationDao(IDbConnection connection) : base("dbo.Location", connection) { }
    }
}