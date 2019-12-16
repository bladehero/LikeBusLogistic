using LikeBusLogistic.BLL.Results;
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
                var routeLocation = UnitOfWork.StoredProcedureDao.GetRouteLocation(routeId, locationId).FirstOrDefault();
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
    }
}
