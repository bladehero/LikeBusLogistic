using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using JwtAuthenticationHelper.Abstractions;
using LikeBusLogistic.BLL;
using LikeBusLogistic.Web.Models.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace LikeBusLogistic.Web.Controllers
{
    public class AccountController : BaseController
    {
        private IJwtTokenGenerator _tokenGenerator;
        private ServiceFactory _serviceFactory;

        public AccountController(IJwtTokenGenerator tokenGenerator, 
            ServiceFactory serviceFactory) : base(serviceFactory)
        {
            _tokenGenerator = tokenGenerator;
            _serviceFactory = serviceFactory;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index(LoginVM model)
        {
            if (!_serviceFactory.AccountManagement.Anonymous)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            try
            {
                var result = _serviceFactory.AccountManagement.SignIn(model.Login, model.Password);
                if (!result.Success)
                {
                    throw new InvalidCredentialException(result.Message);
                }
                var accessTokenResult = _tokenGenerator.GenerateAccessTokenWithClaimsPrincipal(
                    model.Login, AddMyClaims(result.Data.AccountId, result.Data.FirstName, result.Data.RoleName));

                await HttpContext.SignInAsync(accessTokenResult.ClaimsPrincipal,
                    accessTokenResult.AuthProperties);

                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            catch (InvalidCredentialException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                ViewBag.HasError = true;
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Произошла непредвиденная ошибка";
                ViewBag.HasError = true;
            }
            return View("Index", model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _serviceFactory.AccountManagement.LogOff();
            return RedirectToAction(nameof(AccountController.Index), "Account");
        }

        private static IEnumerable<Claim> AddMyClaims(int accountId, string name, string role) =>
        new []
        {
            new Claim(ClaimTypes.NameIdentifier, accountId.ToString()),
            new Claim(ClaimTypes.GivenName, name),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
        };
    }
}