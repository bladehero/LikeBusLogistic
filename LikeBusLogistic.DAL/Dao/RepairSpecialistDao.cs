using LikeBusLogistic.DAL.Models;
using System.Data;
namespace LikeBusLogistic.DAL.Dao
{
    public class RepairSpecialistDao : BaseDao<RepairSpecialist>
    {
        public RepairSpecialistDao(IDbConnection connection) : base("dbo.RepairSpecialist", connection) { }
    }
}