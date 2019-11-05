using LikeBusLogistic.DAL.Models;
using System.Data;
namespace LikeBusLogistic.DAL.Dao
{
    public class DistrictDao : BaseDao<District>
    {
        public DistrictDao(IDbConnection connection) : base("dbo.District", connection) { }
    }
}