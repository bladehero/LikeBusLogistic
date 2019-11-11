using AutoMapper;
using LikeBusLogistic.DAL.Models;
using LikeBusLogistic.DAL.StoredProcedureResults;
using LikeBusLogistic.VM.ViewModels;

namespace LikeBusLogistic.VM.MapperExtensions
{
    public class ServiceMapperExtension : BaseMapperExtension
    {
        public ServiceMapperExtension()
        {
            _config = new MapperConfiguration(cfg =>
            {
                #region Account Management Service
                cfg.CreateMap<Account, AccountVM>();
                cfg.CreateMap<AccountVM, Account>();

                cfg.CreateMap<Role, RoleVM>();
                cfg.CreateMap<RoleVM, Role>();

                cfg.CreateMap<User, UserVM>();
                cfg.CreateMap<UserVM, User>();
                #endregion

                cfg.CreateMap<GetUserAccountById_Result, AccountUserRoleVM>();
                cfg.CreateMap<GetUserAccountByCredentials_Result, AccountUserRoleVM>();
            });
        }
    }
}
