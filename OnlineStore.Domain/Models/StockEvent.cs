using OnlineStore.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Domain.Models
{
	[Table("StockEvents")]
	public class StockEvent
	{
		[Key]
		[Column("Id")]
		public int StockEventID { get; set; }

		public int StockID { get; set; }

		[Required]
		[ForeignKey("StockID")]
		public Stock Stock { get; set; }

		[Required]
		public StockEventTypes Type { get; set; }

		[Column(TypeName = "nvarchar")]
		[MaxLength(512)]
		public string Reason { get; set; }

		[Required]
		public int Quantity { get; set; }

		[Required]
		public DateTimeOffset CreatedAt { get; set; }
	}
}
