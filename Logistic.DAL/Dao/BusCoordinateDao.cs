using Logistic.DAL.Models;
using System.Data;

namespace Logistic.DAL.Dao
{
	public class BusCoordinateDao : BaseDao<BusCoordinate>
	{
		public BusCoordinateDao(IDbConnection connection) : base("dbo.BusCoordinate", connection) { }
	}
}