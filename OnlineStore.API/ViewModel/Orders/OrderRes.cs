namespace OnlineStore.Api.ViewModel.Orders
{
	public class OrderRes
	{
		public int OrderID { get; set; }

		public DateTimeOffset CreatedAt { get; set; }

		public float TotalPrice { get; set; }

		public List<OrderDetailRes> OrderDetails { get; set; }

		public bool IsDeleted { get; set; }
	}
}
