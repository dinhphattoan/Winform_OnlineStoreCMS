namespace OnlineStore.Cms.ViewModels.StockEvent
{
	public class StockEventViewModel : BaseStockEventViewModel
	{
		public string? Name { get; set; }
		public int? Quantity { get; set; }
		public string? Reason { get; set; }
		public string? Type { get; set; }
		public string? CreatedAt { get; set; }
	}
}
