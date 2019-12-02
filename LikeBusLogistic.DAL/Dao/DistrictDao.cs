using LikeBusLogistic.DAL.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LikeBusLogistic.DAL.Dao
{
    public class DistrictDao : BaseDao<District>
    {
        public DistrictDao(IDbConnection connection) : base("dbo.District", connection) { }

        public override IEnumerable<District> FindAll(bool withDeleted = false)
        {
            return base.FindAll(withDeleted).OrderBy(x => x.Name);
        }
    }
}