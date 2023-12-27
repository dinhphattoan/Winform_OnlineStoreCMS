namespace OnlineStore.API.ViewModel.Product
{
	public class ProductRes : BaseProduct
	{
		public string Name { get; set; }

		public string ShortDescription { get; set; }

		public float UnitPrice { get; set; }

		public string CategoryName { get; set; }

		public int Stock { get; set; }

		public string Thumbnail { get; set; }

	}
}
