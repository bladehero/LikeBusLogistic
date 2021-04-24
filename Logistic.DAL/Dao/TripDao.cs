using Dapper;
using Logistic.DAL.Models;
using System.Data;
namespace Logistic.DAL.Dao
{
    public class TripDao : BaseDao<Trip>
    {
        public TripDao(IDbConnection connection) : base("dbo.Trip", connection) { }

        public bool ChangeTripStatus(int id, char status)
        {
            var sql = $"update {TableName} set Status = '{status}' where Id = {id}";
            return Connection.Execute(sql) > 0;
        }
    }
}