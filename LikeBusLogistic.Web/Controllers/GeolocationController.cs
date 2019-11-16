using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LikeBusLogistic.Web.Controllers
{
    public class GeolocationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}