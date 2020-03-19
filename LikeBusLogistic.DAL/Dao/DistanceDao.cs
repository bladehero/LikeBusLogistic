using LikeBusLogistic.DAL.Dao;
using LikeBusLogistic.DAL.Models;
using System.Data;

namespace LikeBusLogistic.DAL.Dao
{
	public class DistanceDao : BaseDao<Distance>
	{
		public DistanceDao(IDbConnection connection) : base("dbo.Distance", connection) { }
	}
}