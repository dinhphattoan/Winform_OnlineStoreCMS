using OnlineStore.Domain.DTO.Product;
using OnlineStore.Domain.DTO.StockEvent;

namespace OnlineStore.Domain.DTO.Stock
{
	public class StockDTO : BaseStockDTO
	{
		public ProductDTO Product { get; set; }

		public StockEventDTO StockEvent { get; set; }
	}
}
