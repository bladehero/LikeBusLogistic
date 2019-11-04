using LikeBusLogistic.DAL.Models;
using System.Data;

namespace LikeBusLogistic.DAL.Dao
{
    public class BaseUserTrackedDao<T> : BaseDao<T> where T : UserTrackedEntity
    {
        public BaseUserTrackedDao(string table, IDbConnection connection) : base(table, connection) { }
    }
}
