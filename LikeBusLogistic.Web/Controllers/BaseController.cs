using LikeBusLogistic.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace LikeBusLogistic.Web.Controllers
{
    public class BaseController : Controller
    {
        protected AccountManagementService AccountManagementService { get; }

        public BaseController(AccountManagementService accountManagementService)
        {
            AccountManagementService = accountManagementService;

        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            AccountManagementService.AccountId =
                int.TryParse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out int accountId)
                ? (int?)accountId
                : null;
        }
    }
}