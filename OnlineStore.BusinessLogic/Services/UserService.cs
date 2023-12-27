using Lombok.NET;
using OnlineStore.DataAccess;
using OnlineStore.Domain.DTO.User;
using OnlineStore.Domain.Exceptions;
using OnlineStore.Domain.Helpers;
using OnlineStore.Domain.Mapper;
using OnlineStore.Domain.Models;
using System.Linq.Expressions;

namespace OnlineStore.BusinessLogic.Services
{
	[RequiredArgsConstructor]
	public partial class UserService
	{
		private readonly UnitOfWork _unitOfWork;
		private readonly IBaseMapper<User, UserDTO> _userDTOMapper;
		private readonly IBaseMapper<UserDTO, User> _userMapper;

		public async Task<UserDTO> GetById(int id)
		{
			try
			{
				User user = await _unitOfWork.UserRepository.GetById(id);
				UserDTO userDTO = _userDTOMapper.MapModel(user);
				return userDTO;
			}
			catch (NotFoundException e)
			{
				throw new HttpStatusCodeException(System.Net.HttpStatusCode.NotFound, e);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public async Task<PaginatedDataViewModel<UserDTO>> GetPaginated(int pageNumber, int pageSize, string? search)
		{
			try
			{
				List<Expression<Func<User, bool>>> filters = new List<Expression<Func<User, bool>>>();
				if (search != null)
				{
					filters.Add(u => u.FirstName.ToLower().Contains(search) || u.LastName.ToLower().Contains(search) || u.Email.ToLower().Contains(search));
				}
				PaginatedDataViewModel<User> paginatedDataViewModel = await _unitOfWork.UserRepository.GetPaginatedData(filters, null, null, pageNumber, pageSize);
				IEnumerable<User> products = paginatedDataViewModel.Data;
				IEnumerable<UserDTO> userDTOs = _userDTOMapper.MapList(products);
				PaginatedDataViewModel<UserDTO> new_paginatedDataViewModel = new PaginatedDataViewModel<UserDTO>(userDTOs, paginatedDataViewModel.TotalCount);
				return new_paginatedDataViewModel;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<UserDTO> Create(UserDTO userDTO)
		{
			try
			{
				User user = _userMapper.MapModel(userDTO);
				user.Password = PasswordHasher.HashPassword(userDTO.Password);
				user = await _unitOfWork.UserRepository.Create(user);
				_unitOfWork.Save();
				UserDTO newUserDTO = _userDTOMapper.MapModel(user);
				return newUserDTO;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<UserDTO> Update(UserDTO userDTO)
		{
			try
			{
				User user = await _unitOfWork.UserRepository.GetById(userDTO.UserID);
				if (userDTO.Password != null)
				{
					user.Password = PasswordHasher.HashPassword(userDTO.Password);
				}
				user.FirstName = userDTO.FirstName;
				user.LastName = userDTO.LastName;
				user.Email = userDTO.Email;
				user.PhoneNumber = userDTO.PhoneNumber;
				user.DateOfBirth = userDTO.DateOfBirth;
				user.Role = userDTO.Role;

				await _unitOfWork.UserRepository.Update(user);
				_unitOfWork.Save();
				UserDTO newUserDTO = _userDTOMapper.MapModel(user);
				return newUserDTO;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<UserDTO> Disable(int id)
		{
			try
			{
				User user = await _unitOfWork.UserRepository.GetById(id);
				user.IsDeleted = true;
				await _unitOfWork.UserRepository.Update(user);
				_unitOfWork.Save();
				UserDTO newUserDTO = _userDTOMapper.MapModel(user);
				return newUserDTO;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public async Task<UserDTO> Enable(int id)
		{
			try
			{
				User user = await _unitOfWork.UserRepository.GetById(id);
				user.IsDeleted = false;
				await _unitOfWork.UserRepository.Update(user);
				_unitOfWork.Save();
				UserDTO newUserDTO = _userDTOMapper.MapModel(user);
				return newUserDTO;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<UserDTO> GetUserByEmail(string email)
		{
			try
			{
				User user = await _unitOfWork.UserRepository.GetUserByEmail(email);
				UserDTO userDTO = _userDTOMapper.MapModel(user);
				return userDTO;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public async Task<string> ChangePassword(int userID, string oldPassword, string newPassword)
		{
			try
			{
				User user = await _unitOfWork.UserRepository.GetById(userID);
				if (PasswordHasher.VerifyPassword(oldPassword, user.Password))
				{
					user.Password = PasswordHasher.HashPassword(newPassword);
					await _unitOfWork.UserRepository.Update(user);
					_unitOfWork.Save();
					return "Change password successfully";
				}
				else
				{
					throw new HttpStatusCodeException(System.Net.HttpStatusCode.BadRequest, "Old password is not correct");
				}
			}
			catch (NotFoundException e)
			{
				throw new HttpStatusCodeException(System.Net.HttpStatusCode.NotFound, e);
			}
		}
	}
}
