﻿using Logistic.BLL.Results;
using Logistic.DAL.Models;
using Logistic.VM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logistic.BLL.Services
{
    public class DriverManagementService : BaseService
    {
        public DriverManagementService(string connection) : base(connection) { }

        public BaseResult<DriverInfoVM> GetDriverInfo(int? driverId)
        {
            var result = new BaseResult<DriverInfoVM>();
            try
            {
                var driver = UnitOfWork.StoredProcedureDao.GetDriverInfo(driverId.Value).FirstOrDefault();
                var driverInfoVM = Mapper.Map<DriverInfoVM>(driver);
                result.Data = driverInfoVM;
                result.Success = true;
                result.Message = GeneralSuccessMessage;
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.Success = false;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }
        public BaseResult<DriverContactVM> GetDriverContact(int? concatId)
        {
            var result = new BaseResult<DriverContactVM>();
            try
            {
                var contact = UnitOfWork.DriverContactDao.FindById(concatId, RoleName == Variables.RoleName.Administrator);
                var driver = UnitOfWork.DriverDao.FindById(contact.Id, RoleName == Variables.RoleName.Administrator);
                var driverContactVM = Mapper.Map<DriverContactVM>(contact);
                driverContactVM.DriverInfo = $"{driver.FirstName} {driver.LastName} {driver.MiddleName}";
                result.Data = driverContactVM;
                result.Success = true;
                result.Message = GeneralSuccessMessage;
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.Success = false;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }

        public BaseResult<IEnumerable<DriverInfoVM>> GetDrivers(bool withDeleted = true)
        {
            var result = new BaseResult<IEnumerable<DriverInfoVM>>();
            try
            {
                var drivers = UnitOfWork.StoredProcedureDao.GetDriverInfo(withDeleted: RoleName == Variables.RoleName.Administrator && withDeleted);
                var driverInfoVMs = Mapper.Map<IEnumerable<DriverInfoVM>>(drivers);
                result.Data = driverInfoVMs;
                result.Success = true;
                result.Message = GeneralSuccessMessage;
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.Success = false;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }
        public BaseResult<IEnumerable<DriverInfoVM>> GetDriversOrderByBus(int? busId, bool withDeleted = true)
        {
            var result = new BaseResult<IEnumerable<DriverInfoVM>>();
            try
            {
                var drivers = UnitOfWork.StoredProcedureDao.GetDriverInfo(withDeleted: RoleName == Variables.RoleName.Administrator && withDeleted);
                var driverInfoVMs = Mapper.Map<IEnumerable<DriverInfoVM>>(drivers);
                if (busId.HasValue)
                {
                    foreach (var item in driverInfoVMs)
                    {
                        item.AttachedOnBus = item.BusId == busId.Value;
                    }
                    driverInfoVMs = driverInfoVMs.OrderByDescending(x => x.AttachedOnBus);
                }
                result.Data = driverInfoVMs;
                result.Success = true;
                result.Message = GeneralSuccessMessage;
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.Success = false;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }
        public BaseResult<IEnumerable<DriverContactVM>> GetDriverContacts()
        {
            var result = new BaseResult<IEnumerable<DriverContactVM>>();
            try
            {
                var driverContactVM = from contact in UnitOfWork.DriverContactDao.FindAll(RoleName == Variables.RoleName.Administrator)
                                      join driver in UnitOfWork.DriverDao.FindAll(RoleName == Variables.RoleName.Administrator)
                                      on contact.DriverId equals driver.Id
                                      select new DriverContactVM
                                      {
                                          Id = contact.Id,
                                          Contact = contact.Contact,
                                          DriverId = driver.Id,
                                          DriverInfo = $"{driver.FirstName} {driver.LastName} {driver.MiddleName}",
                                          IsDeleted = contact.IsDeleted
                                      };
                result.Data = driverContactVM;
                result.Success = true;
                result.Message = GeneralSuccessMessage;
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.Success = false;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }

        public BaseResult MergeDriver(DriverInfoVM driverInfoVM)
        {
            var result = new BaseResult();
            try
            {
                UnitOfWork.StoredProcedureDao.MergeDriver(driverInfoVM.DriverId,
                                                          driverInfoVM.BusId,
                                                          driverInfoVM.FirstName,
                                                          driverInfoVM.LastName,
                                                          driverInfoVM.MiddleName,
                                                          AccountId);
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
        public BaseResult MergeDriverContact(DriverContactVM contactVM)
        {
            var result = new BaseResult();
            try
            {
                var contact = Mapper.Map<DriverContact>(contactVM);
                result.Success = UnitOfWork.DriverContactDao.Merge(contact);
                result.Message = GeneralSuccessMessage;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }

        public BaseResult DeleteOrRestoreDriver(int driverId)
        {
            var result = new BaseResult();
            try
            {
                result.Success = UnitOfWork.DriverDao.DeleteOrRestore(driverId);
                result.Message = GeneralSuccessMessage;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }
        public BaseResult DeleteOrRestoreDriverContact(int contactId)
        {
            var result = new BaseResult();
            try
            {
                result.Success = UnitOfWork.DriverContactDao.DeleteOrRestore(contactId);
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
