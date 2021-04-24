﻿using Logistic.BLL.Results;
using Logistic.DAL.Models;
using Logistic.VM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logistic.BLL.Services
{
    public class BusManagementService : BaseService
    {
        public BusManagementService(string connection) : base(connection) { }

        public BaseResult<VehicleVM> GetVehicle(int? vehicleId)
        {
            var result = new BaseResult<VehicleVM>();
            try
            {
                var vehicle = UnitOfWork.VehicleDao.FindById(vehicleId, RoleName == Variables.RoleName.Administrator);
                result.Data = Mapper.Map<VehicleVM>(vehicle);
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
        public BaseResult<BusVM> GetBus(int? busId)
        {
            var result = new BaseResult<BusVM>();
            try
            {
                var bus = UnitOfWork.BusDao.FindById(busId, RoleName == Variables.RoleName.Administrator);
                var vehicle = UnitOfWork.VehicleDao.FindById(bus.VehicleId, RoleName == Variables.RoleName.Administrator);
                var busVM = new BusVM
                {
                    BusId = bus.Id,
                    CrewCapacity = bus.CrewCapacity,
                    Fullname = $"{vehicle.Producer} {vehicle.Model}",
                    Number = bus.Number,
                    PassengerCapacity = vehicle.PassengerCapacity,
                    VehicleId = vehicle.Id
                };
                result.Data = busVM;
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

        public BaseResult<IEnumerable<VehicleVM>> GetVehicles()
        {
            var result = new BaseResult<IEnumerable<VehicleVM>>();
            try
            {
                var vehicles = UnitOfWork.VehicleDao.FindAll(RoleName == Variables.RoleName.Administrator);
                result.Data = Mapper.Map<IEnumerable<VehicleVM>>(vehicles);
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
        public BaseResult<IEnumerable<BusVM>> GetBuses(bool withDeleted = true)
        {
            var result = new BaseResult<IEnumerable<BusVM>>();
            try
            {
                var buses = from bus in UnitOfWork.BusDao.FindAll(RoleName == Variables.RoleName.Administrator && withDeleted)
                            join vehicle in UnitOfWork.VehicleDao.FindAll(RoleName == Variables.RoleName.Administrator && withDeleted)
                            on bus.VehicleId equals vehicle.Id
                            select new BusVM
                            {
                                BusId = bus.Id,
                                CrewCapacity = bus.CrewCapacity,
                                Fullname = $"{vehicle.Producer} {vehicle.Model}",
                                Number = bus.Number,
                                PassengerCapacity = vehicle.PassengerCapacity,
                                VehicleId = vehicle.Id,
                                IsDeleted = bus.IsDeleted
                            };
                result.Data = buses;
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

        public BaseResult MergeVehicle(VehicleVM vehicleVM)
        {
            var result = new BaseResult();
            try
            {
                var vehicle = Mapper.Map<Vehicle>(vehicleVM);
                if (vehicleVM.Id == 0)
                {
                    vehicle.CreatedBy = AccountId;
                }
                vehicle.ModifiedBy = AccountId;
                result.Success = UnitOfWork.VehicleDao.Merge(vehicle);
                result.Message = GeneralSuccessMessage;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }
        public BaseResult MergeBus(BusVM busVM)
        {
            var result = new BaseResult();
            try
            {
                var bus = Mapper.Map<Bus>(busVM);
                if (bus.Id == 0)
                {
                    bus.CreatedBy = AccountId;
                }
                bus.ModifiedBy = AccountId;
                UnitOfWork.BusDao.Merge(bus);
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

        public BaseResult DeleteOrRestoreVehicle(int vehicleId)
        {
            var result = new BaseResult();
            try
            {
                result.Success = UnitOfWork.VehicleDao.DeleteOrRestore(vehicleId);
                result.Message = GeneralSuccessMessage;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }
        public BaseResult DeleteOrRestoreBus(int busId)
        {
            var result = new BaseResult();
            try
            {
                result.Success = UnitOfWork.BusDao.DeleteOrRestore(busId);
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
