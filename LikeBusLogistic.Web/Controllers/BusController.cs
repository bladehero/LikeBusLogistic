using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LikeBusLogistic.Web.Controllers
{
    [Authorize]
    public class BusController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult _FullInformation()
        {
            System.Threading.Thread.Sleep(2000);
            return PartialView();
        }

        [HttpGet]
        public IActionResult _Vehicles()
        {
            System.Threading.Thread.Sleep(2000);
            return PartialView();
        }

        [HttpGet]
        public IActionResult _Buses()
        {
            System.Threading.Thread.Sleep(2000);
            return PartialView();
        }
    }
}