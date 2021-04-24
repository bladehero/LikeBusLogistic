using AutoMapper;
using Logistic.BLL.Variables;
using Logistic.DAL;
using Logistic.VM.MapperExtensions;
using Logistic.VM.ViewModels;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Logistic.BLL.Services
{
    public abstract class BaseService : IDisposable
    {
        private string _connectionString;

        public const string GeneralSuccessMessage = "Successfully done!";
        public const string GeneralErrorMessage = "Error occured!";

        private IMapper _mapper;
        protected IMapper Mapper => _mapper ?? (_mapper = new ServiceMapperExtension().Mapper);
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
