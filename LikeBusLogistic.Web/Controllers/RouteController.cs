using System;
using System.Collections.Generic;
using System.Linq;
using LikeBusLogistic.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LikeBusLogistic.Web.Models.Routes;
using LikeBusLogistic.VM.ViewModels;
using LikeBusLogistic.Web.Models;
using LikeBusLogistic.BLL.Services.TomTom.Models;
using System.Threading.Tasks;
using LikeBusLogistic.BLL.Results;

namespace LikeBusLogistic.Web.Controllers
{
    [Authorize]
    public class RouteController : BaseController
    {
        public RouteController(ServiceFactory serviceFactory) : base(serviceFactory) { }

        [HttpGet]
        public IActionResult _FullInformation()
        {
            return PartialView();
        }

        [HttpGet]
        public IActionResult _Routes()
        {
            var routes = ServiceFactory.RouteManagement.GetRoutes();

            var model = new RoutesVM
            {
                Routes = routes.Data
            };
            return PartialView(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetRouteLocations(int id, bool reverse = false)
        {
            var data = await GetRouteLocationAsync(id, reverse);
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetDistanceInfo(int locationId1, int locationId2)
        {
            var result = new Result<DistanceVM>();
            try
            {
                var distanceResult = ServiceFactory.RouteManagement.GetDistance(locationId1, locationId2);

                if (!distanceResult.Success)
                {
                    throw new ArgumentException($"An error occured with distanceResult " +
                        $"`{distanceResult}`. Message: {distanceResult.Message}");
                }

                if (distanceResult.Data == null)
                {
                    distanceResult.Data = await GetOrGenerateDistance(locationId1, locationId2);
                }

                result.Data = distanceResult.Data;
                result.Message = distanceResult.Message;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.Message = ex.Message;
                result.Success = false;
            }
            return Json(result);
        }

        [HttpGet]
        public IActionResult _MergeRoute(int id)
        {
            var route = ServiceFactory.RouteManagement.GetRoute(id).Data;

            var model = new MergeRouteVM
            {
                Duration = route.EstimatedDurationInHours,
                Name = route.Name,
                RouteId = id
            };
            return PartialView(model);
        }

        [HttpGet]
        public IActionResult _MergeRouteLocation(MergeRouteLocationVM mergeRouteLocationVM)
        {
            var locations = ServiceFactory.GeolocationManagement.GetLocations().Data;
            var routeLocations = ServiceFactory.RouteManagement.GetRouteLocations(mergeRouteLocationVM.RouteId).Data.ToList();

            mergeRouteLocationVM.Locations = locations;
            mergeRouteLocationVM.RouteLocations = routeLocations;
            return PartialView(mergeRouteLocationVM);
        }


        /// <summary>
        /// Method for inserting a location to route
        /// </summary>
        /// <param name="routeId">Route identifier</param>
        /// <param name="routeLocationId">Route location identifier which is connected with the new location</param>
        /// <param name="locationToAddId">New location identifier in route</param>
        /// <param name="mode">The mode defines how new location will be added to route location</param>
        /// <returns>Base result with list of route locations</returns>
        [HttpGet]
        public async Task<IActionResult> InsertLocationToRoute(int routeId, int routeLocationId, int locationToAddId, MergeRouteLocationMode mode)
        {
            var result = new Result<IEnumerable<RouteLocationVM>>();
            try
            {
                var route = ServiceFactory.RouteManagement.GetRoute(routeId).Data;
                var routeLocations = ServiceFactory.RouteManagement.GetRouteLocations(route.Id).Data;
                var firstRouteLocation = routeLocations.First();

                var routeLocationIds = routeLocations.Select(x => x.CurrentLocationId).ToList();
                var index = routeLocationIds.FindIndex(x => x == routeLocationId);
                if (mode == MergeRouteLocationMode.Prepend)
                {
                    index++;
                }
                routeLocationIds.Insert(index, locationToAddId);

                var routeLocationList = new List<RouteLocationVM>();
                var previousLocationId = (int?)null;
                foreach (var id in routeLocationIds)
                {
                    var routeLocation = new RouteLocationVM
                    {
                        CurrentLocationId = id
                    };
                    if (previousLocationId.HasValue)
                    {
                        var distance = await GetOrGenerateDistance(previousLocationId.Value, id);
                        routeLocation.Distance = distance.TomTomLeg.Summary.LengthInMeters / (double)1000;
                        routeLocation.TomTomInfo = distance.TomTomInfo;
                    }
                    else
                    {
                        routeLocation.CurrentLatitude = firstRouteLocation.CurrentLatitude;
                        routeLocation.CurrentLongitude = firstRouteLocation.CurrentLongitude;
                    }

                    routeLocationList.Add(routeLocation);
                    previousLocationId = id;
                }

                result.Data = routeLocationList;
                result.Message = "Успешно добавлена новая локация!";
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.Success = false;
            }

            return Json(result);
        }

        [HttpGet]
        public IActionResult _CreateRoute(int startLocationId)
        {
            var startLocation = ServiceFactory.GeolocationManagement.GetLocation(startLocationId).Data;

            var model = new CreateRouteVM
            {
                StartLocation = startLocation
            };
            return PartialView(model);
        }

        [HttpPost]
        public IActionResult IsRouteMatch(IEnumerable<LocationVM> locations)
        {
            var result = new Result<bool>();
            try
            {
                var response = ServiceFactory.RouteManagement.IsRouteMatch(locations);
                result.Data = response.Data;
                result.Success = response.Success;
                result.Message = response.Message;
            }
            catch (Exception ex)
            {
                result.Data = false;
                result.Success = false;
            }
            return Json(result);
        }

        [HttpPost]
        public IActionResult CreateRoute(CreateRouteVM createRouteVM)
        {
            var result = new Result();
            try
            {
                var serviceResult = ServiceFactory.RouteManagement.CreateRoute(createRouteVM.Locations, createRouteVM.Name, createRouteVM.EstimatedDurationInHours);

                result.Success = serviceResult.Success;
                result.Message = serviceResult.Message;
            }
            catch (Exception ex)
            {
                result.Success = false;
            }
            return Json(result);
        }

        [HttpPost]
        public IActionResult MergeRoute(MergeRouteVM mergeRouteVM)
        {
            var result = new Result();
            try
            {
                var route = ServiceFactory.RouteManagement.GetRoute(mergeRouteVM.RouteId).Data;
                route.Name = mergeRouteVM.Name;
                route.EstimatedDurationInHours = mergeRouteVM.Duration;

                var serviceResult = ServiceFactory.RouteManagement.MergeRoute(route);

                result.Success = serviceResult.Success;
                result.Message = serviceResult.Message;
            }
            catch (Exception ex)
            {
                result.Success = false;
            }
            return Json(result);
        }

        [HttpPost]
        public IActionResult MergeRouteLocation(MergeRouteLocationVM mergeRouteLocationVM)
        {
            var result = new Result();
            try
            {
                var routeLocations = ServiceFactory.RouteManagement.GetRouteLocations(mergeRouteLocationVM.RouteId).Data;
                var currentRouteLocation = routeLocations.FirstOrDefault(x => x.CurrentLocationId == mergeRouteLocationVM.RouteLocationId);

                var locationToAdd = new RouteLocationVM
                {
                    RouteId = mergeRouteLocationVM.RouteId,
                    CurrentLocationId = mergeRouteLocationVM.LocationToAddId
                };
                var nextId = (int?)null;

                if (mergeRouteLocationVM.Mode == MergeRouteLocationMode.Append)
                {
                    var previous = routeLocations.FirstOrDefault(x => x.CurrentLocationId == currentRouteLocation.PreviousLocationId);
                    locationToAdd.PreviousLocationId = previous?.CurrentLocationId;
                    nextId = currentRouteLocation.CurrentLocationId;
                }
                else if (mergeRouteLocationVM.Mode == MergeRouteLocationMode.Prepend)
                {
                    var next = routeLocations.FirstOrDefault(x => x.PreviousLocationId == currentRouteLocation.CurrentLocationId);
                    locationToAdd.PreviousLocationId = currentRouteLocation.CurrentLocationId;
                    nextId = next?.CurrentLocationId;
                }

                var serviceResult = ServiceFactory.RouteManagement.MergeRouteLocation(locationToAdd, nextId);

                result.Success = serviceResult.Success;
                result.Message = serviceResult.Message;
            }
            catch (Exception ex)
            {
                result.Success = false;
            }
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRouteLocation(int routeId, int locationId)
        {
            var result = new Result<IEnumerable<RouteLocationVM>>();
            try
            {
                var serviceResult = ServiceFactory.RouteManagement.HardDeleteRouteLocation(routeId, locationId);
                var routeLocationResult = await GetRouteLocationAsync(routeId);

                result.Data = routeLocationResult.Data;
                result.Message = routeLocationResult.Message;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.Success = false;
            }
            return Json(result);
        }

        [HttpPost]
        public IActionResult DeleteOrRestoreRoute(int id)
        {
            var result = new Result();
            try
            {
                var deleteOrRestoreRouteResult = ServiceFactory.RouteManagement.DeleteOrRestoreRoute(id);
                result.Success = deleteOrRestoreRouteResult.Success;
                result.Message = deleteOrRestoreRouteResult.Message;
            }
            catch (Exception)
            {
                result.Success = false;
            }
            return Json(result);
        }

        #region Helpers

        private async Task<BaseResult<IEnumerable<RouteLocationVM>>> GetRouteLocationAsync(int routeId, bool reverse = false)
        {
            var routeLocations = ServiceFactory.RouteManagement.GetRouteLocations(routeId, reverse);
            foreach (var routeLocation in routeLocations.Data)
            {
                if (routeLocation.PreviousLocationId.HasValue && routeLocation.TomTomLeg == null)
                {
                    var distance = await GetOrGenerateDistance(routeLocation.PreviousLocationId.Value, routeLocation.CurrentLocationId);
                    routeLocation.Distance = distance.TomTomLeg.Summary.LengthInMeters / (double)1000;
                    routeLocation.TomTomInfo = distance.TomTomInfo;
                }
            }
            return routeLocations;
        }

        private async Task<DistanceVM> GetOrGenerateDistance(int locationId1, int locationId2)
        {
            var location1 = ServiceFactory.GeolocationManagement.GetLocation(locationId1);
            var location2 = ServiceFactory.GeolocationManagement.GetLocation(locationId2);

            if (!location1.Success)
            {
                throw new ArgumentException($"An error occured with location1 `{location1}`. Message: {location1.Message}");
            }
            if (!location2.Success)
            {
                throw new ArgumentException($"An error occured with location2 `{location2}`. Message: {location2.Message}");
            }
            var point1 = new LocationPoint
            {
                Latitude = location1.Data.Latitude,
                Longitude = location1.Data.Longitude
            };
            var point2 = new LocationPoint
            {
                Latitude = location2.Data.Latitude,
                Longitude = location2.Data.Longitude
            };

            var calculateRouteResult = await ServiceFactory.TomTom.CalculateRoute(point1, point2);
            if (!calculateRouteResult.Success)
            {
                throw new ArgumentException($"An error occured with calculateRouteResult " +
                    $"`{calculateRouteResult}`. Message: {calculateRouteResult.Message}");
            }

            var leg = calculateRouteResult.Data.Routes.FirstOrDefault()?.Legs.FirstOrDefault();
            var distance = new DistanceVM
            {
                TomTomInfo = leg == null ? string.Empty : Newtonsoft.Json.JsonConvert.SerializeObject(leg),
                Location1 = location1.Data.Id,
                Location2 = location2.Data.Id
            };

            ServiceFactory.RouteManagement.MergeDistance(distance);
            var distanceResult = ServiceFactory.RouteManagement.GetDistance(distance.Location1, distance.Location2);
            if (!distanceResult.Success)
            {
                throw new ArgumentException($"An error occured with distanceResult" +
                    $"`{distanceResult}` after merging data in database. Message: {distanceResult.Message}");
            }
            return distanceResult.Data;
        }

        #endregion
    }
}