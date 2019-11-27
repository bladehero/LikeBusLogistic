using AutoMapper;
using LikeBusLogistic.BLL.Variables;
using LikeBusLogistic.DAL;
using LikeBusLogistic.VM.MapperExtensions;
using LikeBusLogistic.VM.ViewModels;
using System;
using System.Data;
using System.Data.SqlClient;

namespace LikeBusLogistic.BLL.Services
{
    public abstract class BaseService : IDisposable
    {
        private string _connectionString;

        public const string GeneralSuccessMessage = "Успешно выполнено!";
        public const string GeneralErrorMessage = "Произошла ошибка!";

        protected IMapper Mapper => new ServiceMapperExtension().Mapper;
        protected UnitOfWork UnitOfWork { get; set; }
        protected IDbConnection Connection => new SqlConnection(_connectionString);

        public int? AccountId { get; set; }
        public bool Anonymous => !AccountId.HasValue && AccountUserRole == null;
        public AccountUserRoleVM AccountUserRole =>
        AccountId.HasValue
        ? Mapper.Map<AccountUserRoleVM>(UnitOfWork.StoredProcedureDao.GetUserAccountById(AccountId.Value))
        : null;
        public RoleName RoleName
        {
            get
            {
                RoleName role;
                if (AccountUserRole?.RoleName == RoleName.Administrator.ToString())
                {
                    role = RoleName.Administrator;
                }
                else if (AccountUserRole?.RoleName == RoleName.Moderator.ToString())
                {
                    role = RoleName.Moderator;
                }
                else if (AccountUserRole?.RoleName == RoleName.Operator.ToString())
                {
                    role = RoleName.Operator;
                }
                else
                {
                    role = RoleName.Unknown;
                }
                return role;
            }
        }

        public BaseService(string connection)
        {
            _connectionString = connection;
            UnitOfWork = new UnitOfWork(Connection);
        }

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }
    }
}
