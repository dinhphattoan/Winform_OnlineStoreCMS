using Lombok.NET;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Api.Authentication;
using OnlineStore.Api.ViewModel.Auth;
using OnlineStore.Api.ViewModel.User;
using OnlineStore.BusinessLogic.Services;
using OnlineStore.Domain.DTO.User;
using OnlineStore.Domain.Exceptions;
using OnlineStore.Domain.Mapper;
using System.Net;

namespace OnlineStore.Api.Controllers
{
	[Route("api/")]
	[ApiController]
	[RequiredArgsConstructor]
	[Authorize]
	public partial class UserController : ControllerBase
	{
		private readonly UserService _userService;
		private readonly IUserProvider _userProvider;
		private readonly IBaseMapper<UserDTO, UserRes> _userResMapper;
		[HttpGet]
		[Route("user")]
		public async Task<IActionResult> Get()
		{
			try
			{
				int userId = _userProvider.GetUserId();
				UserDTO user = await _userService.GetById(userId);
				return Ok(_userResMapper.MapModel(user));
			}
			catch (NotFoundException e)
			{
				return StatusCode((int)HttpStatusCode.BadGateway, new { error = "Not Found", message = e.Message });
			}
		}

		[HttpPut]
		[Route("change-password")]
		public async Task<IActionResult> ChangePassword(ChangePassword req)
		{
			try
			{
				int userId = _userProvider.GetUserId();
				string message = await _userService.ChangePassword(userId, req.OldPassword, req.NewPassword);
				return Ok(new { message });
			}
			catch (NotFoundException e)
			{
				return StatusCode((int)HttpStatusCode.BadGateway, new { error = "Not Found", message = e.Message });
			}
		}
	}
}
