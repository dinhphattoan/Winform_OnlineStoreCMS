using Lombok.NET;
using OnlineStore.BusinessLogic.Services.Interface;
using OnlineStore.DataAccess;
using OnlineStore.Domain.DTO.User;
using OnlineStore.Domain.Exceptions;
using OnlineStore.Domain.Helpers;
using OnlineStore.Domain.Mapper;
using OnlineStore.Domain.Models;
using System.Net;
namespace OnlineStore.BusinessLogic.Services.Auth
{
	[RequiredArgsConstructor]
	public partial class RegisterService : IRegisterService
	{
		private readonly UnitOfWork _unitOfWork;
		private readonly IBaseMapper<UserDTO, User> _userMapper;
		private readonly IBaseMapper<User, UserDTO> _userDTOMapper;
		public async Task<bool> IsEmaiExist(string email)
		{
			User user = await _unitOfWork.UserRepository.GetUserByEmail(email);
			if (user != null)
			{
				return false;
			}
			return true;
		}

		public async Task<UserDTO> Register(UserDTO request)
		{
			UserDTO user = new UserDTO() { Email = request.Email, FirstName = request.FirstName, LastName = request.LastName, Password = request.Password };
			user.Role = Domain.Enums.Role.CUSTOMER;
			user.Password = PasswordHasher.HashPassword(user.Password);
			var new_user = await _unitOfWork.UserRepository.Create(_userMapper.MapModel(user));
			_unitOfWork.Save();
			if (new_user != null)
			{
				return _userDTOMapper.MapModel(new_user);
			}
			else
			{
				throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "Please check username or password");
			}
		}

	}
}
