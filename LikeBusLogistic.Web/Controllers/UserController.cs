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
    public class UserController : BaseController
    {
        public UserController(ServiceFactory serviceFactory) : base(serviceFactory)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}