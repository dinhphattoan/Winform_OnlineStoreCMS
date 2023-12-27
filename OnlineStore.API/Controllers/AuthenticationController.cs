using Lombok.NET;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Api.Authentication;
using OnlineStore.Api.ViewModel.Auth;

namespace OnlineStore.Api.Controllers
{
	[RequiredArgsConstructor]
	[Route("api/")]
	[ApiController]
	public partial class AuthenticationController : ControllerBase
	{
		private readonly IAuthentication _authentication;
		[AllowAnonymous]
		[HttpPost]
		[Route("login")]
		public async Task<IActionResult> Login([FromBody] LoginReq request)
		{
			var response = await _authentication.Login(request);

			return Ok(response);
		}

		[AllowAnonymous]
		[HttpPost]
		[Route("register")]
		public async Task<IActionResult> Register([FromBody] RegisterReq request)
		{
			var response = await _authentication.Register(request);

			return Ok(response);
		}
	}
}
