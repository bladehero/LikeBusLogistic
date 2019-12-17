using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LikeBusLogistic.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LikeBusLogistic.Web.Models.Routes;
using LikeBusLogistic.VM.ViewModels;

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
        public IActionResult _MergeRouteLocation(int routeId, int locationToAddId, int routeLocationId, MergeRouteLocationMode mode)
        {
            var locations = ServiceFactory.GeolocationManagement.GetLocations().Data;
            var routeLocations = ServiceFactory.RouteManagement.GetRouteLocations(routeId).Data;

            var model = new MergeRouteLocationVM
            {
                LocationToAdd = new LocationVM
                {
                    Id = locationToAddId
                },
                RouteLocation = new RouteLocationVM
                {
                    RouteId = routeId,
                    RouteLocationId = routeLocationId
                },
                Mode = mode,
                Locations = locations,
                RouteLocations = routeLocations
            };
            return PartialView(model);
        }
    }
}