using OnlineStore.Cms.ViewModels.Product;

namespace OnlineStore.Cms.ViewModels.OrderDetail
{
	public class OrderDetailViewModel : BaseOrderDetailViewModel
	{
		public BaseProductViewModel Product { get; set; }

		public float UnitPrice { get; set; }

		public int Quantity { get; set; }
	}
}
