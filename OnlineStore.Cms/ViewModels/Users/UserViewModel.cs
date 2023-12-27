using CloudinaryDotNet.Actions;

namespace OnlineStore.Cms.ViewModels.Users
{
	public class UserViewModel : BaseUserViewModel
	{
		public string Email { get; set; }

		public string DateOfBirth { get; set; }

		public string PhoneNumber { get; set; }

		public Role Role { get; set; }
	}
}
