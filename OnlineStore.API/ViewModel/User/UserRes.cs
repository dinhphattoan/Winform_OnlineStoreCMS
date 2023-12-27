namespace OnlineStore.Api.ViewModel.User
{
	public class UserRes
	{
		public int UserID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string? PhoneNumber { get; set; }
		public string Email { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string? CivilianId { get; set; }
	}
}
