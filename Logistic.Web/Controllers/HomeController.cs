using Logistic.BLL;
using Logistic.Web.Models.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Logistic.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public HomeController(ServiceFactory serviceFactory) : base(serviceFactory)
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetLocations()
        {
            var locations = ServiceFactory.GeolocationManagement.GetLocations(false);
            return Json(locations);
        }

        [HttpGet]
        public IActionResult _LocationPopup(int id, int? routeId, bool isRoute = false)
        {
            var location = ServiceFactory.GeolocationManagement.GetLocation(id).Data;
            var route = ServiceFactory.RouteManagement.GetRoute(routeId).Data;
            var routeLocations = ServiceFactory.RouteManagement.GetRouteLocations(routeId).Data;
            var selected = ServiceFactory.RouteManagement.GetRouteLocation(routeId, id).Data;

            var model = new LocationPopupVM
            {
                Location = location,
                Route = route,
                RouteLocation = selected,
                IsRoute = isRoute,
                IsFirstInRoute = routeLocations.FirstOrDefault()?.CurrentLocationId == selected?.CurrentLocationId,
                IsLastInRoute = routeLocations.LastOrDefault()?.CurrentLocationId == selected?.CurrentLocationId
            };
            return PartialView(model);
        }
    }
}