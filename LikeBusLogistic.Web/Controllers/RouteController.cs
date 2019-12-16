using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LikeBusLogistic.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LikeBusLogistic.Web.Models.Routes;

namespace LikeBusLogistic.Web.Controllers
{
    [Authorize]
    public class RouteController : BaseController
    {
        public RouteController(ServiceFactory serviceFactory) : base(serviceFactory) { }

        public IActionResult _FullInformation()
        {
            var routes = ServiceFactory.RouteManagement.GetRoutes();

            var model = new FullInformationVM
            {
                Routes = routes.Data
            };
            return PartialView(model);
        }
    }
}