using OnlineStore.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Cms.ViewModels.Stock
{
	public class StockReqViewModel
	{
		[Required]
		public int StockID { get; set; }

		[Required]
		public int Quantity { get; set; }

		[Required]
		public string? Reason { get; set; }

		[Required]
		public StockEventTypes type { get; set; }
	}
}
