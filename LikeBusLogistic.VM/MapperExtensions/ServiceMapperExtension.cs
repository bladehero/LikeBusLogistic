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

                cfg.CreateMap<GetUserAccountById_Result, AccountUserRoleVM>();
                cfg.CreateMap<GetUserAccountByCredentials_Result, AccountUserRoleVM>();

                cfg.CreateMap<AccountUserRoleVM, GetUserAccountById_Result>();
                cfg.CreateMap<AccountUserRoleVM, GetUserAccountByCredentials_Result>();
                #endregion

                #region Bus Management Service
                cfg.CreateMap<Vehicle, VehicleVM>();
                cfg.CreateMap<VehicleVM, Vehicle>();

                cfg.CreateMap<Bus, BusVM>().AfterMap((m, vm) => m.Id = vm.BusId);
                cfg.CreateMap<BusVM, Bus>().AfterMap((vm, m) => vm.BusId = m.Id);
                #endregion

                #region Driver Management Service
                cfg.CreateMap<DriverInfoVM, Driver>().AfterMap((vm, m) => m.Id = vm.BusId);
                cfg.CreateMap<DriverInfoVM, BusDriver>();
                cfg.CreateMap<DriverInfoVM, GetDriverInfo_Result>();
                cfg.CreateMap<GetDriverInfo_Result, DriverInfoVM>();

                cfg.CreateMap<DriverContact, DriverContactVM>();
                cfg.CreateMap<DriverContactVM, DriverContact>();
                #endregion
            });
        }
    }
}
