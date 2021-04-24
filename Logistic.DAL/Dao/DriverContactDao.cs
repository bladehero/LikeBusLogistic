using Logistic.DAL.Models;
using System.Collections.Generic;
using System.Data;
namespace Logistic.DAL.Dao
{
    public class DriverContactDao : BaseDao<DriverContact>
    {
        public DriverContactDao(IDbConnection connection) : base("dbo.DriverContact", connection) { }

        public IEnumerable<DriverContact> GetContacts(int driverId)
        {
            return Query($"{SelectFromString} where DriverId = {driverId}");
        }
    }
}