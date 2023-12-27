using OnlineStore.Domain.Enums;

namespace OnlineStore.Domain.DTO.User
{
	public class BaseUserDTO
	{
		public int UserID { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }

		public Role Role { get; set; }

		public bool IsDeleted { get; set; }
	}
}
