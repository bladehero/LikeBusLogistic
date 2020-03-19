using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LikeBusLogistic.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LikeBusLogistic.Web.Models.Routes;
using LikeBusLogistic.VM.ViewModels;
using LikeBusLogistic.Web.Models;

namespace LikeBusLogistic.Web.Controllers
{
    [Authorize]
    public class RouteController : BaseController
    {
        public RouteController(ServiceFactory serviceFactory) : base(serviceFactory) { }

        [HttpGet]
        public IActionResult _FullInformation()
        {
            var routes = ServiceFactory.RouteManagement.GetRoutes();

            var model = new FullInformationVM
            {
                Routes = routes.Data
            };
            return PartialView(model);
        }

        [HttpGet]
        public IActionResult GetRouteLocations(int id)
        {
            var routeLocations = ServiceFactory.RouteManagement.GetRouteLocations(id);
            var points = routeLocations.Data.Select(x => new BLL.Services.TomTom.LocationPoint { Latitude = x.CurrentLatitude, Longitude = x.CurrentLongitude });
            return Json(routeLocations);
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
            var routeLocations = ServiceFactory.RouteManagement.GetRouteLocations(mergeRouteLocationVM.RouteId).Data;

            mergeRouteLocationVM.Locations = locations;
            mergeRouteLocationVM.RouteLocations = routeLocations;
            return PartialView(mergeRouteLocationVM);
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
                else if(mergeRouteLocationVM.Mode == MergeRouteLocationMode.Prepend)
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
        public IActionResult DeleteRouteLocation(int routeId, int locationId)
        {
            var result = new Result();
            try
            {
                var serviceResult = ServiceFactory.RouteManagement.HardDeleteRouteLocation(routeId, locationId);

                result.Success = serviceResult.Success;
                result.Message = serviceResult.Message;
            }
            catch (Exception ex)
            {
                result.Success = false;
            }
            return Json(result);
        }
    }
}