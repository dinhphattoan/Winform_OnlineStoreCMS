﻿namespace OnlineStore.Cms.ViewModels.Users
{
	public class UserListViewModel : BaseUserViewModel
	{

		public string Email { get; set; }

		public string Role { get; set; }

		public bool IsDeleted { get; set; }
	}
}
