using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace OnlineStore.Domain.Models
{
	[Table("Orders")]
	public class Order
	{
		[Key]
		[Column("Id")]
		public int OrderID { get; set; }
		public int? ClerkID { get; set; }
		[ForeignKey("ClerkID")]
		[AllowNull]
		public User? Clerk { get; set; }

		public int CustomerID { get; set; }
		[ForeignKey("CustomerID")]
		public User? Customer { get; set; }

		[Required]
		public DateTimeOffset CreatedAt { get; set; }

		public bool IsDeleted { get; set; }

		public List<OrderDetail> OrderDetails { get; set; }
	}
}
