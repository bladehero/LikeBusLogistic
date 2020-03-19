using LikeBusLogistic.DAL.Models;
using System.Data;

namespace LikeBusLogistic.DAL.Dao
{
	public class BusCoordinateDao : BaseDao<BusCoordinate>
	{
		public BusCoordinateDao(IDbConnection connection) : base("dbo.BusCoordinate", connection) { }
	}
}