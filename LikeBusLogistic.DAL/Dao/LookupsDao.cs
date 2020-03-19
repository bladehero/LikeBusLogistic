using LikeBusLogistic.DAL.Dao;
using LikeBusLogistic.DAL.Models;
using System.Data;

namespace LikeBusLogistic.DAL.Dao
{
	public class LookupsDao : BaseDao<Lookups>
	{
		public LookupsDao(IDbConnection connection) : base("dbo.Lookups", connection) { }
	}
}