using LikeBusLogistic.BLL.Results;
using LikeBusLogistic.DAL.Models;
using LikeBusLogistic.DAL.StoredProcedureResults;
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
        public BaseResult<IEnumerable<RouteLocationVM>> GetRouteLocations(int? routeId, bool reverse = false)
        {
            var result = new BaseResult<IEnumerable<RouteLocationVM>>();
            try
            {
                var routeLocations = UnitOfWork.StoredProcedureDao.GetRouteLocation(routeId);
                if (reverse)
                {
                    var temporary = new List<GetRouteLocation_Result>();
                    var previous = (GetRouteLocation_Result)null;
                    foreach (var item in routeLocations.Reverse())
                    {
                        var distance = (DistanceVM)null;
                        if (previous == null)
                        {
                            item.PreviousLocationId = null;
                            item.PreviousFullName = null;
                            item.PreviousName = null;
                            item.PreviousCityId = null;
                            item.PreviousCityName = null;
                            item.PreviousCountryId = null;
                            item.PreviousCountryName = null;
                            item.PreviousDistrictId = null;
                            item.PreviousDistrictName = null;
                            item.PreviousIsCarRepair = null;
                            item.PreviousIsParking = null;
                            item.PreviousLatitude = null;
                            item.PreviousLongitude = null;
                        }
                        else
                        {
                            item.PreviousLocationId = previous.CurrentLocationId;
                            item.PreviousFullName = previous.CurrentFullName;
                            item.PreviousName = previous.CurrentName;
                            item.PreviousCityId = previous.CurrentCityId;
                            item.PreviousCityName = previous.CurrentCityName;
                            item.PreviousCountryId = previous.CurrentCountryId;
                            item.PreviousCountryName = previous.CurrentCountryName;
                            item.PreviousDistrictId = previous.CurrentDistrictId;
                            item.PreviousDistrictName = previous.CurrentDistrictName;
                            item.PreviousIsCarRepair = previous.CurrentIsCarRepair;
                            item.PreviousIsParking = previous.CurrentIsParking;
                            item.PreviousLatitude = previous.CurrentLatitude;
                            item.PreviousLongitude = previous.CurrentLongitude;
                            var distanceResult = GetDistance(item.PreviousLocationId.Value, item.CurrentLocationId.Value);
                            if (!distanceResult.Success)
                            {
                                throw new ArgumentException(distanceResult.Message);
                            }
                            distance = distanceResult.Data;
                        }
                        item.TomTomInfo = distance?.TomTomInfo;
                        temporary.Add(item);
                        previous = item;
                    }
                    routeLocations = temporary;
                }
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="locations">Route location to match.</param>
        /// <returns>in Data true if any route was matched with all locations</returns>
        public BaseResult<bool> IsRouteMatch(IEnumerable<LocationVM> locations)
        {
            var result = new BaseResult<bool>();
            bool isMatch = false;
            try
            {
                var list = locations.ToList();
                var routes = UnitOfWork.RouteDao.GetRouteByLocationCount(list.Count);
                foreach (var route in routes)
                {
                    isMatch = true;
                    var routeLocations = UnitOfWork.StoredProcedureDao.GetRouteLocation(route.Id).ToList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (routeLocations[i].CurrentLocationId != list[i].Id)
                        {
                            isMatch = false;
                            break;
                        }
                    }

                    if (isMatch)
                    {
                        break;
                    }
                }

                result.Data = isMatch;
                result.Success = true;
                result.Message = GeneralSuccessMessage;
            }
            catch (Exception ex)
            {
                result.Data = false;
                result.Success = false;
                result.Message = GeneralErrorMessage;
            }
            return result;
        }

        public BaseResult<DistanceVM> GetDistance(int location1Id, int location2Id)
        {
            var result = new BaseResult<DistanceVM>();
            try
            {
                var distance = UnitOfWork.StoredProcedureDao.GetDistance(location1Id, location2Id).FirstOrDefault();
                var data = Mapper.Map<DistanceVM>(distance);
                result.Data = data;
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
        public BaseResult MergeDistance(DistanceVM distanceVM)
        {
            var result = new BaseResult();
            try
            {
                var distance = Mapper.Map<Distance>(distanceVM);
                UnitOfWork.DistanceDao.Merge(distance);

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

        public BaseResult CreateRoute(IEnumerable<LocationVM> locations, string name, float? estimatedDurationInHours = null)
        {
            var result = new BaseResult();
            try
            {
                var routeId = UnitOfWork.RouteDao.Insert(new Route
                {
                    ArrivalId = locations.LastOrDefault().Id,
                    DepartureId = locations.FirstOrDefault().Id,
                    Name = name,
                    EstimatedDurationInHours = estimatedDurationInHours,
                    CreatedBy = AccountId,
                    ModifiedBy = AccountId
                });

                var previousLocation = (LocationVM)null;
                foreach (var location in locations)
                {
                    UnitOfWork.RouteLocationDao.Insert(new RouteLocation
                    {
                        RouteId = routeId,
                        CurrentLocationId = location.Id,
                        PreviousLocationId = previousLocation?.Id,
                        Distance = location.Distance,
                        CreatedBy = AccountId,
                        ModifiedBy = AccountId
                    });
                    previousLocation = location;
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
        public BaseResult MergeRoute(RouteVM routeVM)
        {
            var result = new BaseResult();
            try
            {
                var route = Mapper.Map<Route>(routeVM);
                UnitOfWork.RouteDao.Merge(route);

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

        public BaseResult DeleteOrRestoreRoute(int routeId)
        {
            var result = new BaseResult();
            try
            {
                result.Success = UnitOfWork.RouteDao.DeleteOrRestore(routeId);
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
