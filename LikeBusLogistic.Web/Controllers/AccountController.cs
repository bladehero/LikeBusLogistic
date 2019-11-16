using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using JwtAuthenticationHelper.Abstractions;
using JwtAuthenticationHelper.Types;
using LikeBusLogistic.BLL.Services;
using LikeBusLogistic.Web.Models.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LikeBusLogistic.Web.Controllers
{
    public class AccountController : Controller
    {
        private IJwtTokenGenerator _tokenGenerator;
        private AccountManagementService _accountManagementService;
        private TokenOptions _tokenOptions;

        public AccountController(IJwtTokenGenerator tokenGenerator, AccountManagementService accountManagementService, TokenOptions tokenOptions)
        {
            _tokenGenerator = tokenGenerator;
            _accountManagementService = accountManagementService;
            _tokenOptions = tokenOptions;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index(LoginVM model)
        {
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            try
            {
                var result = _accountManagementService.SignIn(model.Login, model.Password);
                if (!result.Success)
                {
                    throw new InvalidCredentialException(result.Message);
                }
                var accessTokenResult = _tokenGenerator.GenerateAccessTokenWithClaimsPrincipal(
                    model.Login, AddMyClaims(result.Data.FirstName, result.Data.RoleName));

                await HttpContext.SignInAsync(accessTokenResult.ClaimsPrincipal,
                    accessTokenResult.AuthProperties);

                return RedirectToAction(nameof(BusController.Index), "Bus");
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

            return RedirectToAction(nameof(AccountController.Index), "Account");
        }

        private static IEnumerable<Claim> AddMyClaims(string name, string role) =>
        new []
        {
            new Claim(ClaimTypes.GivenName, name),
            new Claim(ClaimTypes.Role, role)
        };
    }
}