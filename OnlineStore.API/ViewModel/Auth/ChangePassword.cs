using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Api.ViewModel.Auth
{
	public class ChangePassword
	{
		[Required]
		public string OldPassword { get; set; }

		[Required]
		public string NewPassword { get; set; }
	}
}
