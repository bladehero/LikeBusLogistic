using LikeBusLogistic.DAL;
using System.Data;

namespace LikeBusLogistic.BLL.Services
{
    public abstract class BaseService
    {
        protected UnitOfWork UnitOfWork { get; set; }

        public int? UserId { get; set; }
        public bool Anonymous => !UserId.HasValue;
        public 

        public BaseService(IDbConnection connection)
        {
            UnitOfWork = new UnitOfWork(connection);
        }
    }
}
