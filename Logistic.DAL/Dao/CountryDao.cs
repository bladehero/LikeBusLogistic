using Logistic.DAL.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Logistic.DAL.Dao
{
    public class CountryDao : BaseDao<Country>
    {
        public CountryDao(IDbConnection connection) : base("dbo.Country", connection) { }

        public override IEnumerable<Country> FindAll(bool withDeleted = false)
        {
            return base.FindAll(withDeleted).OrderBy(x => x.Name);
        }
    }
}