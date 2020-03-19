using LikeBusLogistic.DAL.Dao;
using LikeBusLogistic.DAL.Models;
using System.Data;

namespace LikeBusLogistic.DAL.Dao
{
	public class LookupValuesDao : BaseDao<LookupValues>
	{
		public LookupValuesDao(IDbConnection connection) : base("dbo.LookupValues", connection) { }
	}
}