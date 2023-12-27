using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Domain.Models
{
	[Table("Stocks")]
	public class Stock
	{
		[Key]
		[Column("Id")]
		public int StockID { get; set; }

		[Required]
		public int Quantity { get; set; }

		public int ProductID { get; set; }

		[ForeignKey("ProductID")]
		[Required]
		public Product Product { get; set; }
	}
}
