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
    public class DriverController : BaseController
    {
        public DriverController(ServiceFactory serviceFactory) : base(serviceFactory)
        {
        }

        public IActionResult _FullInformation()
        {
            return View();
        }

        public IActionResult _Information()
        {
            return View();
        }

        public IActionResult _Contacts()
        {
            return View();
        }
    }
}