using OnlineStore.Domain.DTO.Stock;

namespace OnlineStore.Domain.DTO.StockEvent
{
	public class StockEventDTO : BaseStockEventDTO
	{
		public StockDTO Stock { get; set; }

		public string Reason { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
	}
}
