using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace OnlineStore.Domain.Models
{
	[Table("Products")]
	public class Product
	{
		[Key]
		[Column("Id")]
		public int ProductID { get; set; }

		[Required]
		[MaxLength(100)]
		public string Name { get; set; }

		[Column(TypeName = "nvarchar(max)")]
		public string? Description { get; set; }

		[MaxLength(256)]
		[Column(TypeName = "nvarchar")]
		public string? Thumbnail { get; set; }

		[Required]
		[Column(TypeName = "float")]
		public float UnitPrice { get; set; }

		public int CreateByID { get; set; }

		[Required]
		[ForeignKey("CreateByID")]
		public User CreateBy { get; set; }

		[Required]
		[Column(TypeName = "datetime")]
		public DateTime CreatAt { get; set; }

		public int CategoryID { get; set; }

		[ForeignKey("CategoryID")]
		[Required]
		public virtual Category Category { get; set; }

		[AllowNull]
		public virtual List<ProductImage> ProductImages { get; set; }

		public bool IsDeleted { get; set; }

		public Stock? Stock { get; set; }
	}
}
