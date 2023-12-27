using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.BusinessLogic.Services;
using OnlineStore.Cms.ViewModels.Auth;
using OnlineStore.Domain.DTO.User;
using OnlineStore.Domain.Enums;
using System.Security.Claims;

namespace OnlineStore.Cms.Controllers
{
	[AllowAnonymous]
	public class AuthenticationController : Controller
	{

		private readonly ILogger<AuthenticationController> _logger;
		private readonly LoginService _loginService;

		public AuthenticationController(ILogger<AuthenticationController> logger, LoginService customAuthentication)
		{
			_logger = logger;
			_loginService = customAuthentication;
		}

		[Route("/login")]
		[HttpGet]
		public IActionResult Login()
		{
			LoginViewModel objLoginModel = new LoginViewModel();
			return View("Login", objLoginModel);
		}

		[Route("/login")]
		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel objLoginModel)
		{
			if (ModelState.IsValid)
			{
				UserDTO user = await _loginService.Login(objLoginModel.Email, objLoginModel.Password);
				if (user != null && user.Role != Role.CUSTOMER)
				{
					var claims = new List<Claim>() {
						new Claim(ClaimTypes.Email, user.Email),
						new Claim(ClaimTypes.Name, user.FirstName +" "+user.LastName),
						new Claim(ClaimTypes.Role, user.Role.ToString()),
					};
					//Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme    
					var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
					//Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity    
					var principal = new ClaimsPrincipal(identity);
					//SignInAsync is a Extension method for Sign in a principal for the specified scheme.    
					await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
					{
						IsPersistent = objLoginModel.RememberLogin
					});
					if (user.Role == Role.ADMIN)
					{
						return RedirectToAction("Index", "Users");
					}
					else if (user.Role == Role.CLERK)
					{
						return RedirectToAction("Index", "Products");
					}
				}
				else
				{
					ViewBag.Message = "Access is denied";
					ViewBag.MessageType = ToastType.Error;
					return View("Login", objLoginModel);
				}
			}
			return View("Login", objLoginModel);
		}

		[Route("/logout")]
		[Authorize]
		public async Task<IActionResult> LogOut()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Login", "Authentication");
		}
	}
}
