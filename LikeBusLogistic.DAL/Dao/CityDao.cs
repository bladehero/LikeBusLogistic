using LikeBusLogistic.DAL.Models;
using System.Data;
namespace LikeBusLogistic.DAL.Dao
{
    public class CityDao : BaseDao<City>
    {
        public CityDao(IDbConnection connection) : base("dbo.City", connection) { }
    }
}