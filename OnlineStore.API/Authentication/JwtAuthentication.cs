using Lombok.NET;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OnlineStore.Api.ViewModel.Auth;
using OnlineStore.BusinessLogic.Services.Interface;
using OnlineStore.Domain.DTO.User;
using OnlineStore.Domain.Exceptions;
using OnlineStore.Domain.Mapper;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace OnlineStore.Api.Authentication
{
	[RequiredArgsConstructor]
	public partial class JwtAuthentication : IAuthentication
	{
		private readonly IConfiguration _configuration;
		private readonly ILoginService _loginService;
		private readonly IRegisterService _registerService;
		private readonly IBaseMapper<RegisterReq, UserDTO> _userDTOMapper;
		private readonly IBaseMapper<UserDTO, BaseUserRes> _userResMapper;
		public async Task<string> Login(LoginReq loginReq)
		{
			var user = await _loginService.Login(loginReq.Email, loginReq.Password);
			var authClaims = new List<Claim>
				{
					new(ClaimTypes.NameIdentifier, user.UserID.ToString()),
					new(ClaimTypes.Name, user.FirstName+" "+user.LastName),
					new(ClaimTypes.Email, user.Email),
					new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				};

			var token = GetToken(authClaims);

			return $"{{\"token\": \"{new JwtSecurityTokenHandler().WriteToken(token)}\", \"user\": {_userResMapper.MapModel(user)}}}";
		}

		private JwtSecurityToken GetToken(IEnumerable<Claim> authClaims)
		{
			var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

			var token = new JwtSecurityToken(
				issuer: _configuration["JWT:ValidIssuer"],
				audience: _configuration["JWT:ValidAudience"],
				expires: DateTime.Now.AddHours(3),
				claims: authClaims,
				signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

			return token;
		}

		private string GetErrorsText(IEnumerable<IdentityError> errors)
		{
			return string.Join(", ", errors.Select(error => error.Description).ToArray());
		}

		public async Task<string> Register(RegisterReq request)
		{
			if (!await _registerService.IsEmaiExist(request.Email))
			{
				throw new HttpStatusCodeException(HttpStatusCode.Conflict, $"User with email {request.Email} already exists.");
			}
			var user = _userDTOMapper.MapModel(request);
			var result = await _registerService.Register(user);

			if (result == null)
			{
				throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, $"Unable to register user {request.Email}");
			}

			return await Login(new LoginReq { Email = request.Email, Password = request.Password });
		}
	}
}
