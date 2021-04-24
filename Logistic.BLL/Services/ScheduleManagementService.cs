using Logistic.BLL.Results;
using Logistic.DAL.Models;
using Logistic.VM.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Logistic.BLL.Services
{
    public class ScheduleManagementService : BaseService
    {
        private RouteManagementService _routeManagementService;

        public ScheduleManagementService(string connection, RouteManagementService routeManagementService)
            : base(connection) => (_routeManagementService) = (routeManagementService);

        public BaseResult<ScheduleVM> GetSchedule(int? scheduleId)
        {
            var result = new BaseResult<ScheduleVM>();
            try
            {
                var schedule = UnitOfWork.StoredProcedureDao.GetSchedule(scheduleId.Value, withDeleted: RoleName == Variables.RoleName.Administrator).FirstOrDefault();
                result.Data = Mapper.Map<ScheduleVM>(schedule);
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
        public BaseResult<IEnumerable<ScheduleVM>> GetSchedules(bool withDeleted = true)
        {
            var result = new BaseResult<IEnumerable<ScheduleVM>>();
            try
            {
                var schedule = UnitOfWork.StoredProcedureDao.GetSchedule(withDeleted: RoleName == Variables.RoleName.Administrator && withDeleted);
                result.Data = Mapper.Map<IEnumerable<ScheduleVM>>(schedule);
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
        public BaseResult<IEnumerable<ScheduleRouteLocationVM>> GetScheduleRouteLocations(int? scheduleId)
        {
            var result = new BaseResult<IEnumerable<ScheduleRouteLocationVM>>();
            try
            {
                var scheduleInfo = UnitOfWork.StoredProcedureDao.GetScheduleInfo(scheduleId);
                result.Data = Mapper.Map<IEnumerable<ScheduleRouteLocationVM>>(scheduleInfo);
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

        public BaseResult MergeSchedule(ScheduleVM scheduleVM, IEnumerable<ScheduleRouteLocationVM> scheduleRouteLocationVMs = null)
        {
            var result = new BaseResult();
            try
            {
                var isNew = scheduleVM.Id == 0;
                var schedule = Mapper.Map<Schedule>(scheduleVM);
                if (isNew)
                {
                    schedule.CreatedBy = AccountId;
                }
                schedule.ModifiedBy = AccountId;
                UnitOfWork.ScheduleDao.Merge(schedule);
                var scheduleRouteLocations = Mapper.Map<IEnumerable<ScheduleLocation>>(scheduleRouteLocationVMs);
                if (scheduleRouteLocationVMs?.Count() == 0 && isNew)
                {
                    var routeLocations = _routeManagementService.GetRouteLocations(scheduleVM.RouteId).Data;
                    var list = new List<ScheduleLocation>(routeLocations.Count());
                    foreach (var routeLocation in routeLocations)
                    {
                        list.Add(new ScheduleLocation
                        {
                            CreatedBy = AccountId,
                            ModifiedBy = AccountId,
                            PreviousLocationId = routeLocation.PreviousLocationId,
                            CurrentLocationId = routeLocation.CurrentLocationId
                        });
                    }
                    scheduleRouteLocations = list;
                }
                foreach (var routeLocation in scheduleRouteLocations)
                {
                    routeLocation.ModifiedBy = AccountId;
                    routeLocation.CreatedBy = AccountId;
                }
                UnitOfWork.ScheduleLocationDao.MergeScheduleRouteLocations(schedule.Id, scheduleRouteLocations);

                result.Success = true;
                result.Message = GeneralSuccessMessage;
            }
            catch(SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    result.Message = "Such a record already exists. Try changing the name of the schedule!";
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }

        public BaseResult DeleteOrRestoreSchedule(int scheduleId)
        {
            var result = new BaseResult();
            try
            {
                result.Success = UnitOfWork.ScheduleDao.DeleteOrRestore(scheduleId);
                result.Message = GeneralSuccessMessage;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }

        public BaseResult<IEnumerable<ScheduleVM>> UpdateNeedsSyncStatusByRouteId(int routeId)
        {
            var result = new BaseResult<IEnumerable<ScheduleVM>>();
            try
            {
                var schedules = UnitOfWork.ScheduleDao.UpdateNeedsSyncByRouteId(routeId);
                result.Data = Mapper.Map<IEnumerable<ScheduleVM>>(schedules);
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

        public BaseResult<bool> HasConfirmedTripsByRouteId(int? routeId)
        {
            var result = new BaseResult<bool>();
            try
            {
                var hasConfirmedTrips = UnitOfWork.StoredProcedureDao.HasConfirmedTripsByRouteId(routeId);
                result.Data = hasConfirmedTrips;
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
    }
}
