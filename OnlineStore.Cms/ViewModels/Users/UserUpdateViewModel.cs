﻿using OnlineStore.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Cms.ViewModels.Users
{
	public class UserUpdateViewModel : BaseUserViewModel
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
		public string? NewPassword { get; set; }

		[MaxLength(20)]
		public string? PhoneNumber { get; set; }

		public DateTime? DateOfBirth { get; set; }

		[Required]
		public Role Role { get; set; }
	}
}
