using OnlineStore.API.ViewModel.Product;

namespace OnlineStore.Api.ViewModel.Product
{
	public class ProductDetailRes : BaseProduct
	{
		public string Name { get; set; }

		public string Description { get; set; }

		public float UnitPrice { get; set; }

		public string CategoryName { get; set; }

		public int Stock { get; set; }

		public string Thumbnail { get; set; }
		public List<string> Images { get; set; }
	}
}
