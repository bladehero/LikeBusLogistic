using LikeBusLogistic.DAL.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LikeBusLogistic.DAL.Dao
{
    public class CityDao : BaseDao<City>
    {
        public CityDao(IDbConnection connection) : base("dbo.City", connection) { }

        public override IEnumerable<City> FindAll(bool withDeleted = false)
        {
            return base.FindAll(withDeleted).OrderBy(x => x.Name);
        }
    }
}