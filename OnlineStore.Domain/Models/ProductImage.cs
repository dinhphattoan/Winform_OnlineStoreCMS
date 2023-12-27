using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Domain.Models
{
	[Table("ProductImages")]
	public class ProductImage
	{

		[Key]
		[Column("Id")]
		public int ProductImageID { get; set; }

		public int Order { get; set; }

		[Required]
		[MaxLength(256)]
		[Column(TypeName = "nvarchar")]
		public string Path { get; set; }

		public int ProductID { get; set; }

		[ForeignKey("ProductID")]
		public Product Product { get; set; }
	}
}
