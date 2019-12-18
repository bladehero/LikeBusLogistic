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
            return Json(routeLocations);
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
                    EstimatedDurationInHours = 1,
                    StopDurationInHours = 0,
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