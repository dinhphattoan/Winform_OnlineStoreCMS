using OnlineStore.Domain.DTO.User;

namespace OnlineStore.BusinessLogic.Services.Interface
{
	public interface IRegisterService
	{
		Task<bool> IsEmaiExist(string email);

		Task<UserDTO> Register(UserDTO request);
	}
}
