using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Domain.Models
{
	[Table("OrderDetails")]
	public class OrderDetail
	{
		[Key]
		[Column("Id")]
		public int OrderDetailID { get; set; }

		public int OrderID { get; set; }

		[Required]
		[ForeignKey("OrderID")]
		public Order Order { get; set; }

		public int ProductID { get; set; }

		[Required]
		[ForeignKey("ProductID")]
		public Product Product { get; set; }

		[Required]
		public int Quantity { get; set; }

		[Required]
		[Column(TypeName = "float")]
		public float UnitPrice { get; set; }
	}
}
