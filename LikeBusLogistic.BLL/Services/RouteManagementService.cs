using LikeBusLogistic.BLL.Results;
using LikeBusLogistic.DAL.Models;
using LikeBusLogistic.VM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LikeBusLogistic.BLL.Services
{
    public class RouteManagementService : BaseService
    {
        public RouteManagementService(string connection) : base(connection) { }

        public BaseResult<RouteVM> GetRoute(int? routeId)
        {
            var result = new BaseResult<RouteVM>();
            try
            {
                var route = UnitOfWork.StoredProcedureDao.GetRoute(routeId.Value, RoleName == Variables.RoleName.Administrator).FirstOrDefault();
                result.Data = Mapper.Map<RouteVM>(route);
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
        public BaseResult<RouteLocationVM> GetRouteLocation(int? routeId, int? locationId)
        {
            var result = new BaseResult<RouteLocationVM>();
            try
            {
                var routeLocation = UnitOfWork.StoredProcedureDao.GetRouteLocation(routeId, locationId.Value).FirstOrDefault();
                result.Data = Mapper.Map<RouteLocationVM>(routeLocation);
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
        public BaseResult<IEnumerable<RouteVM>> GetRoutes()
        {
            var result = new BaseResult<IEnumerable<RouteVM>>();
            try
            {
                var routes = UnitOfWork.StoredProcedureDao.GetRoute(withDeleted: RoleName == Variables.RoleName.Administrator);
                result.Data = Mapper.Map<IEnumerable<RouteVM>>(routes);
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
        public BaseResult<IEnumerable<RouteLocationVM>> GetRouteLocations(int? routeId)
        {
            var result = new BaseResult<IEnumerable<RouteLocationVM>>();
            try
            {
                var routeLocations = UnitOfWork.StoredProcedureDao.GetRouteLocation(routeId);
                result.Data = Mapper.Map<IEnumerable<RouteLocationVM>>(routeLocations);
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
        public BaseResult MergeRouteLocation(RouteLocationVM routeLocationVM, int? nextRouteLocationId)
        {
            var result = new BaseResult();
            try
            {
                var nextVM = GetRouteLocation(routeLocationVM.RouteId, nextRouteLocationId).Data;
                if (nextVM != null)
                {
                    nextVM.PreviousLocationId = routeLocationVM.CurrentLocationId;
                    var next = Mapper.Map<RouteLocation>(nextVM);
                    UnitOfWork.RouteLocationDao.Update(next);
                }

                var routeLocation = Mapper.Map<RouteLocation>(routeLocationVM);
                UnitOfWork.RouteLocationDao.Merge(routeLocation);

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
        public BaseResult HardDeleteRouteLocation(int routeId, int locationId)
        {
            var result = new BaseResult();
            try
            {
                var routeLocations = GetRouteLocations(routeId).Data;
                var routeLocation = routeLocations.FirstOrDefault(x => x.CurrentLocationId == locationId);

                var previousVM = routeLocations.FirstOrDefault(x => x.CurrentLocationId == routeLocation.PreviousLocationId);
                var nextVM = routeLocations.FirstOrDefault(x => x.PreviousLocationId == locationId);
                if (nextVM != null)
                {
                    nextVM.PreviousLocationId = previousVM?.CurrentLocationId;
                    var next = Mapper.Map<RouteLocation>(nextVM);
                    UnitOfWork.RouteLocationDao.Update(next);
                }

                result.Success = UnitOfWork.RouteLocationDao.HardDelete(routeLocation.RouteLocationId);
                result.Message = result.Success ? GeneralSuccessMessage : GeneralErrorMessage;
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
