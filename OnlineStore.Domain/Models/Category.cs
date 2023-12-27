using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Domain.Models
{
	[Table("Categories")]
	public class Category
	{
		[Key]
		[Column("Id")]
		public int CategoryID { get; set; }

		[Required]
		[MaxLength(100)]
		public string Name { get; set; }

		[Column(TypeName = "nvarchar")]
		[MaxLength(2048)]
		public string Desc { get; set; }

		[MaxLength(256)]
		[Column(TypeName = "nvarchar")]
		public string? Image { get; set; }

		public bool IsDeleted { get; set; }
	}
}
