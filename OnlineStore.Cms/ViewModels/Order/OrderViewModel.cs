using OnlineStore.Cms.ViewModels.OrderDetail;

namespace OnlineStore.Cms.ViewModels.Order
{
	public class OrderViewModel : BaseOrderViewModel
	{
		public string? ClerkName { get; set; }
		public string? CustomerName { get; set; }
		public bool IsDeleted { get; set; }
		public float TotalPrice { get; set; }
		public List<OrderDetailViewModel>? OrderItems { get; set; }

		public string? Reason { get; set; }
	}
}
