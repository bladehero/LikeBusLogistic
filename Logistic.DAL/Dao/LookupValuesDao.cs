using Logistic.DAL.Models;
using System.Data;

namespace Logistic.DAL.Dao
{
	public class LookupValuesDao : BaseDao<LookupValues>
	{
		public LookupValuesDao(IDbConnection connection) : base("dbo.LookupValues", connection) { }
	}
}