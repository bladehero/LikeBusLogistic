using System.Security.Claims;
using LikeBusLogistic.BLL;
using LikeBusLogistic.Web.Variables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LikeBusLogistic.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected ServiceFactory ServiceFactory { get; set; }

        public BaseController(ServiceFactory serviceFactory) => ServiceFactory = serviceFactory;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var id = int.TryParse(context
                                  .HttpContext
                                  .User
                                  .FindFirstValue(ClaimTypes.NameIdentifier)
                                  , out int accountId)
                     ? (int?)accountId
                     : null;
            ServiceFactory.AccountId = id;

            RoleNames? role = null;
            if (ServiceFactory.AccountManagement.AccountUserRole?.RoleName == RoleNames.Administrator.ToString())
            {
                role = RoleNames.Administrator;
            }
            else if (ServiceFactory.AccountManagement.AccountUserRole?.RoleName == RoleNames.Moderator.ToString())
            {
                role = RoleNames.Moderator;
            }
            else if (ServiceFactory.AccountManagement.AccountUserRole?.RoleName == RoleNames.Operator.ToString())
            {
                role = RoleNames.Operator;
            }
            ViewBag.AccountRole = role;

            base.OnActionExecuting(context);
        }

        protected override void Dispose(bool disposing)
        {
            ServiceFactory.Dispose();
            base.Dispose(disposing);
        }
    }
}