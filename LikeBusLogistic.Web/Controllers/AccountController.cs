using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using LikeBusLogistic.BLL.Services;
using LikeBusLogistic.Web.Models;
using LikeBusLogistic.Web.Models.Account;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LikeBusLogistic.Web.Controllers
{
    public class AccountController : Controller
    {
        private AccountManagementService _accountManagementService;
        private AuthOptions _authOptions;

        public AccountController(AccountManagementService accountManagementService, AuthOptions authOptions)
        {
            _accountManagementService = accountManagementService;
            _authOptions = authOptions;
        }

        public IActionResult Index(LoginVM model)
        {
            return View(model);
        }

        public IActionResult Login(LoginVM model)
        {
            try
            {
                var result = _accountManagementService.SignIn(model.Login, model.Password);
                if (!result.Success)
                {
                    throw new InvalidCredentialException(result.Message);
                }
                var token = _getJwtSercurityToken(model.Login, result.Data.RoleName);

                var cookieOptions = new CookieOptions
                {
                    Path = "/",
                    HttpOnly = false,
                    IsEssential = true,
                    Expires = DateTime.Now.AddMonths(1)
                };
                Response.Cookies.Append(JwtBearerDefaults.AuthenticationScheme, token, cookieOptions);
                ViewBag.RedirectToHome = true;
            }
            catch (InvalidCredentialException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                ViewBag.HasError = true;
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "Произошла непредвиденная ошибка";
                ViewBag.HasError = true;
            }
            return View("Index", model);
        }

        private string _getJwtSercurityToken(string login, string role)
        {
            var claimsIdentity = (ClaimsIdentity)null;
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
            };
            claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            var jwt = new JwtSecurityToken(
                    issuer: _authOptions.Issuer,
                    audience: _authOptions.Audience,
                    notBefore: DateTime.UtcNow,
                    claims: claimsIdentity.Claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(_authOptions.LifeTime)),
                    signingCredentials: new SigningCredentials(_authOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}