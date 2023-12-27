namespace OnlineStore.Api.ViewModel.Auth
{
	public class BaseUserRes
	{
		public int id { get; set; }
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public override string ToString()
		{
			return $"{{\"id\":\"{id}\",\"firstName\":\"{FirstName}\",\"lastName\":\"{LastName}\"}}";
		}
	}
}
