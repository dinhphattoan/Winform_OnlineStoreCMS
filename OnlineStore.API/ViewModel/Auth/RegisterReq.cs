using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Api.ViewModel.Auth
{
	public class RegisterReq
	{
		[Required]
		[MaxLength(100)]
		public string FirstName { get; set; }

		[MaxLength(100)]
		public string LastName { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[DataType(DataType.Password)]
		[Required]
		public string Password { get; set; }
	}
}
