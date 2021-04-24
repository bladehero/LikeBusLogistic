﻿using Logistic.BLL.Results;
using Logistic.DAL.Models;
using Logistic.VM.ViewModels;
using System;

namespace Logistic.BLL.Services
{
    public class AccountManagementService : BaseService
    {
        public AccountManagementService(string connection) : base(connection) { }

        public BaseResult<AccountUserRoleVM> SignIn(string login, string password)
        {
            var result = new BaseResult<AccountUserRoleVM>();
            try
            {
                var accountUserRole = UnitOfWork.StoredProcedureDao.GetUserAccountByCredentials(login, password);
                if (accountUserRole != null)
                {
                    var accountUserRoleVM = Mapper.Map<AccountUserRoleVM>(accountUserRole);

                    UnitOfWork.AccountDao.UpdateDateModified(accountUserRole.AccountId);

                    result.Data = accountUserRoleVM;
                    result.Success = true;
                    result.Message = GeneralSuccessMessage;
                }
                else
                {
                    result.Message = "Wrong login or password!";
                }
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.Success = false;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }
        public BaseResult LogOff()
        {
            var result = new BaseResult();
            try
            {
                UnitOfWork.AccountDao.UpdateDateModified(AccountId.Value);
                AccountId = null;

                result.Success = true;
                result.Message = GeneralSuccessMessage;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }

        public BaseResult MergeAccount(AccountVM accountVM)
        {
            var result = new BaseResult();
            try
            {
                var account = Mapper.Map<Account>(accountVM);
                result.Success = UnitOfWork.AccountDao.Merge(account);
                result.Message = GeneralSuccessMessage;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }
        public BaseResult MergeUser(UserVM userVM)
        {
            var result = new BaseResult();
            try
            {
                var user = Mapper.Map<User>(userVM);
                result.Success = UnitOfWork.UserDao.Merge(user);
                result.Message = GeneralSuccessMessage;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }
        public BaseResult MergeRole(RoleVM roleVM)
        {
            var result = new BaseResult();
            try
            {
                var role = Mapper.Map<Role>(roleVM);
                result.Success = UnitOfWork.RoleDao.Merge(role);
                result.Message = GeneralSuccessMessage;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }

        public BaseResult DeleteOrRestoreAccount(int id)
        {
            var result = new BaseResult();
            try
            {
                result.Success = UnitOfWork.AccountDao.DeleteOrRestore(id);
                result.Message = GeneralSuccessMessage;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }
        public BaseResult DeleteOrRestoreUser(int id)
        {
            var result = new BaseResult();
            try
            {
                result.Success = UnitOfWork.UserDao.DeleteOrRestore(id);
                result.Message = GeneralSuccessMessage;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }
        public BaseResult DeleteOrRestoreRole(int id)
        {
            var result = new BaseResult();
            try
            {
                result.Success = UnitOfWork.RoleDao.DeleteOrRestore(id);
                result.Message = GeneralSuccessMessage;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }
    }
}
