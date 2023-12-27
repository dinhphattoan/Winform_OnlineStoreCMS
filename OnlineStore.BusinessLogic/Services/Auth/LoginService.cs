using Lombok.NET;
using OnlineStore.BusinessLogic.Services.Interface;
using OnlineStore.DataAccess;
using OnlineStore.Domain.DTO.User;
using OnlineStore.Domain.Exceptions;
using OnlineStore.Domain.Helpers;
using OnlineStore.Domain.Mapper;
using OnlineStore.Domain.Models;
using System.Net;

namespace OnlineStore.BusinessLogic.Services
{
	[RequiredArgsConstructor]
	public partial class LoginService : ILoginService
	{
		private readonly UnitOfWork _unitOfWork;
		private readonly IBaseMapper<User, UserDTO> _userDTOMapper;
		public async Task<UserDTO> Login(string email, string password)
		{
			User user = await _unitOfWork.UserRepository.GetUserByEmail(email);
			if (user != null && VerifyPassword(password, user.Password) && user.IsDeleted != true)
			{
				return _userDTOMapper.MapModel(user);
			}
			else
			{
				throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Please check username or password");
			}
		}

		private bool VerifyPassword(string password, string hashedPassword)
		{
			return PasswordHasher.VerifyPassword(password, hashedPassword);
		}
	}
}
