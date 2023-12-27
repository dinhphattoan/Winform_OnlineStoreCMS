using OnlineStore.Domain.Enums;

namespace OnlineStore.Domain.DTO.StockEvent

{
	public class BaseStockEventDTO
	{
		public int StockEventID { get; set; }

		public StockEventTypes Type { get; set; }

		public int Quantity { get; set; }

	}
}
