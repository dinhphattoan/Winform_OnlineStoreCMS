using OnlineStore.Domain.DTO.User;

namespace OnlineStore.BusinessLogic.Services.Interface
{
	public interface ILoginService
	{
		Task<UserDTO> Login(string email, string password);
	}
}
