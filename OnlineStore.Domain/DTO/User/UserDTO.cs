namespace OnlineStore.Domain.DTO.User
{
	public class UserDTO : BaseUserDTO
	{
		public string Password { get; set; }

		public string PhoneNumber { get; set; }

		public DateTime DateOfBirth { get; set; }
	}
}
