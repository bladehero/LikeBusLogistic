using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LikeBusLogistic.BLL;
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
            var locations = ServiceFactory.GeolocationManagement.GetLocations();
            return Json(locations);
        }
    }
}