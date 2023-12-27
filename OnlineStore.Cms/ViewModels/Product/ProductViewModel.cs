namespace OnlineStore.Cms.ViewModels.Product
{
	public class ProductViewModel : BaseProductViewModel
	{
		public int? CategoryID { get; set; }

		public string? CategoryName { get; set; }

		public string? ShortDescription { get; set; }

		public string? Description { get; set; }

		public int? Quantity { get; set; }

		public List<ProductImageViewModel> ProductImages { get; set; }

		public string? createByName { get; set; }

		public string? UnitPrice { get; set; }

		public bool IsDeleted { get; set; }

	}
}
