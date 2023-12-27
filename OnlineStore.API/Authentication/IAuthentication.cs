using OnlineStore.Api.ViewModel.Auth;

namespace OnlineStore.Api.Authentication
{
	public interface IAuthentication
	{
		Task<string> Register(RegisterReq request);
		Task<string> Login(LoginReq request);
	}
}
