using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace OnlineStore.Domain.Models
{
	[Table("Users")]
	[Index(nameof(Email), IsUnique = true)]
	public class User
	{
		[Key]
		[Column("Id")]
		public int UserID { get; set; }

		[Required]
		[MaxLength(100)]
		[PersonalData]
		public string FirstName { get; set; }

		[MaxLength(100)]
		[PersonalData]
		public string LastName { get; set; }

		[Column(TypeName = "nvarchar")]
		[MaxLength(100)]
		[PersonalData]
		public string? CivilianId { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		[PersonalData]
		public string Email { get; set; }

		[DataType(DataType.Password)]
		[PersonalData]
		public string Password { get; set; }

		[Column(TypeName = "nvarchar")]
		[MaxLength(20)]
		[PersonalData]
		[AllowNull]
		public string? PhoneNumber { get; set; }

		[PersonalData]
		[AllowNull]
		public DateTime? DateOfBirth { get; set; }

		[Required]
		public Role Role { get; set; }

		public bool IsDeleted { get; set; }

	}
}
