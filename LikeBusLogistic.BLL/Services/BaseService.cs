using AutoMapper;
using LikeBusLogistic.DAL;
using LikeBusLogistic.VM.MapperExtensions;
using LikeBusLogistic.VM.ViewModels;
using System.Data;

namespace LikeBusLogistic.BLL.Services
{
    public abstract class BaseService
    {
        public const string GeneralSuccessMessage = "Успешно выполнено!";
        public const string GeneralErrorMessage = "Успешно выполнено!";

        protected IMapper Mapper => new ServiceMapperExtension().Mapper;
        protected UnitOfWork UnitOfWork { get; set; }

        public int? AccountId { get; set; }
        public bool Anonymous => !AccountId.HasValue;
        public AccountUserRoleVM AccountUserRole => 
            AccountId.HasValue 
            ? Mapper.Map<AccountUserRoleVM>(UnitOfWork.StoredProcedureDao.GetUserAccountById(AccountId.Value)) 
            : null;

        public BaseService(IDbConnection connection)
        {
            UnitOfWork = new UnitOfWork(connection);
        }
    }
}
