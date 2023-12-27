using OnlineStore.Domain.DTO.Product;

namespace OnlineStore.Domain.DTO.OrderDetail
{
	public class OrderDetailDTO : BaseOrderDetail
	{
		public ProductDTO Product { get; set; }
		public int Quantity { get; set; }
		public float UnitPrice { get; set; }
	}
}
