using LikeBusLogistic.DAL.Models;
using System.Data;
namespace LikeBusLogistic.DAL.Dao
{
    public class CountryDao : BaseDao<Country>
    {
        public CountryDao(IDbConnection connection) : base("dbo.Country", connection) { }
    }
}