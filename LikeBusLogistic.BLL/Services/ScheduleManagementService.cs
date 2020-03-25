using LikeBusLogistic.BLL.Results;
using LikeBusLogistic.DAL.Models;
using LikeBusLogistic.VM.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace LikeBusLogistic.BLL.Services
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
                var schedule = UnitOfWork.StoredProcedureDao.GetSchedule(scheduleId.Value, RoleName == Variables.RoleName.Administrator).FirstOrDefault();
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
                var scheduleRouteLocations = Mapper.Map<IEnumerable<ScheduleRouteLocation>>(scheduleRouteLocationVMs);
                if (scheduleRouteLocationVMs?.Count() == 0 && isNew)
                {
                    var routeLocations = _routeManagementService.GetRouteLocations(scheduleVM.RouteId).Data;
                    var list = new List<ScheduleRouteLocation>(routeLocations.Count());
                    foreach (var routeLocation in routeLocations)
                    {
                        list.Add(new ScheduleRouteLocation
                        {
                            CreatedBy = AccountId,
                            ModifiedBy = AccountId,
                            RouteLocationId = routeLocation.RouteLocationId
                        });
                    }
                    scheduleRouteLocations = list;
                }
                foreach (var routeLocation in scheduleRouteLocations)
                {
                    routeLocation.ScheduleId = schedule.Id;
                    routeLocation.ModifiedBy = AccountId;
                    routeLocation.CreatedBy = AccountId;
                }
                UnitOfWork.ScheduleRouteLocationDao.MergeScheduleRouteLocations(scheduleRouteLocations);

                result.Success = true;
                result.Message = GeneralSuccessMessage;
            }
            catch(SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    result.Message = "Такая запись уже существует. Попробуйте изменить название расписания!";
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
    }
}
