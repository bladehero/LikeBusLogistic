using LikeBusLogistic.DAL.Models;
using System.Data;
namespace LikeBusLogistic.DAL.Dao
{
    public class TripDao : BaseDao<Trip>
    {
        public TripDao(IDbConnection connection) : base("dbo.Trip", connection) { }
    }
}