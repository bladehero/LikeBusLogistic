using Logistic.BLL.Results;
using Logistic.DAL.Models;
using Logistic.VM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logistic.BLL.Services
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

        public BaseResult<BusVM> GetLastBusForTrip(int? tripId)
        {
            var result = new BaseResult<BusVM>();
            try
            {
                var tripBus = UnitOfWork.TripBusDao.LastOrDefault(x => x.TripId == tripId.Value);
                var bus = UnitOfWork.BusDao.FindById(tripBus?.BusId);
                result.Data = Mapper.Map<BusVM>(bus);
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
                var tripDrivers = UnitOfWork.TripBusDriverDao.Find(x => x.TripBusId == tripBus.Id);
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

        public BaseResult MergeTrip(TripVM tripVM, IEnumerable<DriverInfoVM> drivers)
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
                var schedule = UnitOfWork.ScheduleDao.FindById(trip.ScheduleId);
                var firstRouteLocation = UnitOfWork.StoredProcedureDao.GetRouteLocation(schedule.RouteId).FirstOrDefault();
                UnitOfWork.TripDao.Merge(trip);

                var tripBus = new TripBus
                {
                    BusId = tripVM.BusId,
                    TripId = trip.Id,
                    LocationId = firstRouteLocation.CurrentLocationId.Value,
                    CreatedBy = AccountId,
                    ModifiedBy = AccountId
                };
                UnitOfWork.TripBusDao.Merge(tripBus);

                foreach (var driver in drivers)
                {
                    var tripBusDriver = new TripBusDriver
                    {
                        DriverId = driver.DriverId.Value,
                        TripBusId = tripBus.Id,
                        CreatedBy = AccountId,
                        ModifiedBy = AccountId
                    };
                    UnitOfWork.TripBusDriverDao.Merge(tripBusDriver);
                }

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

        public BaseResult ChangeStatusTrip(int tripId, TripStatus status)
        {
            var result = new BaseResult();
            try
            {
                result.Success = UnitOfWork.TripDao.ChangeTripStatus(tripId, status.ToString()[0]);
                result.Message = GeneralSuccessMessage;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }

        public BaseResult<int> CountOfTripsHaveToBeStarted()
        {
            var result = new BaseResult<int>();
            try
            {
                result.Data = UnitOfWork.StoredProcedureDao.CountOfTripsHaveToBeStarted();
                result.Success = true;
                result.Message = GeneralSuccessMessage;
            }
            catch (Exception ex)
            {
                result.Data = 0;
                result.Success = false;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }

        public BaseResult<IEnumerable<TripVM>> StartPendingTrips()
        {
            var result = new BaseResult<IEnumerable<TripVM>>();
            try
            {
                var trips = UnitOfWork.StoredProcedureDao.StartPendingTrips();
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

        public BaseResult<IEnumerable<TripVM>> DelayStartedTrips()
        {
            var result = new BaseResult<IEnumerable<TripVM>>();
            try
            {
                var trips = UnitOfWork.StoredProcedureDao.DelayStartedTrips();
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
    }
}
