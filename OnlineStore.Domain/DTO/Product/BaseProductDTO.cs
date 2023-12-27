using OnlineStore.Domain.DTO.Stock;

namespace OnlineStore.Domain.DTO.Product
{
	public class BaseProductDTO
	{
		public int ProductID { get; set; }

		public string? Name { get; set; }

		public string? Description { get; set; }

		public string? Thumbnail { get; set; }

		public BaseStockDTO Stock { get; set; }

		public float UnitPrice { get; set; }
	}
}
