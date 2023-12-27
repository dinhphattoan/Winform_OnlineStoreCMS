using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Api.ViewModel.Auth
{
	public class LoginReq
	{
		[Required]
		[Display(Name = "Email")]
		[DataType(DataType.EmailAddress, ErrorMessage = "Email is not valid")]
		public string Email { get; set; }

		[Required]
		[Display(Name = "Password")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Display(Name = "Remember me")]
		public bool RememberLogin { get; set; }
	}
}
