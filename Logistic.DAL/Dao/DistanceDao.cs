using Logistic.DAL.Models;
using System.Data;

namespace Logistic.DAL.Dao
{
	public class DistanceDao : BaseDao<Distance>
	{
		public DistanceDao(IDbConnection connection) : base("dbo.Distance", connection) { }
	}
}