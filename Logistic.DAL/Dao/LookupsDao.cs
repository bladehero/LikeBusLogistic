using Logistic.DAL.Models;
using System.Data;

namespace Logistic.DAL.Dao
{
	public class LookupsDao : BaseDao<Lookups>
	{
		public LookupsDao(IDbConnection connection) : base("dbo.Lookups", connection) { }
	}
}