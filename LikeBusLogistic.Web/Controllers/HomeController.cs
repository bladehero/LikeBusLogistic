using LikeBusLogistic.BLL;
using LikeBusLogistic.Web.Models.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LikeBusLogistic.Web.Controllers
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
        public IActionResult _LocationPopup(int id)
        {
            var location = ServiceFactory.GeolocationManagement.GetLocation(id).Data;

            var model = new LocationPopupVM
            {
                Location = location
            };
            return PartialView(model);
        }
    }
}