using Logistic.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Logistic.Web.Controllers
{
    [Authorize]
    public class LogisticController : BaseController
    {
        public LogisticController(ServiceFactory serviceFactory) : base(serviceFactory)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}