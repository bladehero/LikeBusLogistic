using LikeBusLogistic.BLL.Results;
using LikeBusLogistic.DAL.Models;
using LikeBusLogistic.VM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LikeBusLogistic.BLL.Services
{
    public class TripManagementService : BaseService
    {
        public TripManagementService(string connection) : base(connection)
        {
        }


        public BaseResult<TripVM> GetTrip(int? tripId)
        {
            var result = new BaseResult<TripVM>();
            try
            {
                var trip = UnitOfWork.StoredProcedureDao.GetTrips(tripId.Value, withDeleted: RoleName == Variables.RoleName.Administrator).FirstOrDefault();
                result.Data = Mapper.Map<TripVM>(trip);
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
        public BaseResult<IEnumerable<DriverInfoVM>> GetLastDriverInfoForTrip(int? tripId)
        {
            var result = new BaseResult<IEnumerable<DriverInfoVM>>();
            try
            {
                var tripBus = UnitOfWork.TripBusDao.LastOrDefault(x => x.TripId == tripId.Value);
                var tripDrivers = UnitOfWork.TripBusDriverDao.Find(x=>x.TripBusId == tripBus.Id);
                var driverList = new List<DriverInfoVM>();
                foreach (var item in tripDrivers)
                {
                    var driver = UnitOfWork.StoredProcedureDao.GetDriverInfo(item?.DriverId).FirstOrDefault();
                    if (driver != null)
                    {
                        var driverVM = Mapper.Map<DriverInfoVM>(driver);
                        driverList.Add(driverVM);
                    }
                }

                result.Data = Mapper.Map<IEnumerable<DriverInfoVM>>(driverList);
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

        public BaseResult<IEnumerable<TripVM>> GetTrips(string status = null)
        {
            var result = new BaseResult<IEnumerable<TripVM>>();
            try
            {
                var trips = UnitOfWork.StoredProcedureDao.GetTrips(null, status, RoleName == Variables.RoleName.Administrator);
                result.Data = Mapper.Map<IEnumerable<TripVM>>(trips);
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

        public BaseResult MergeTrip(TripVM tripVM)
        {
            var result = new BaseResult();
            try
            {
                var trip = Mapper.Map<Trip>(tripVM);
                if (tripVM.Id == 0)
                {
                    trip.CreatedBy = AccountId;
                }
                trip.ModifiedBy = AccountId;
                result.Success = UnitOfWork.TripDao.Merge(trip);
                result.Message = GeneralSuccessMessage;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }

        public BaseResult DeleteOrRestoreTrip(int tripId)
        {
            var result = new BaseResult();
            try
            {
                result.Success = UnitOfWork.TripDao.DeleteOrRestore(tripId);
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
