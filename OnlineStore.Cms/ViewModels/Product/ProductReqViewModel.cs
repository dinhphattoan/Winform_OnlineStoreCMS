using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace OnlineStore.Cms.ViewModels.Product
{
	public class ProductReqViewModel
	{
		public int? ProductID { get; set; }
		[Required]
		public string? Name { get; set; }

		[Required]
		public string? Description { get; set; }

		[Required]
		public float? UnitPrice { get; set; }

		[Required]
		public int? CategoryID { get; set; }

		public string? Thumbnail { get; set; }

		public List<IFormFile>? Images { get; set; }

		public int Quantity { get; set; }

		[AllowNull]
		public string? deleteImages { get; set; }

		public override string ToString()
		{
			String images = "";
			foreach (var item in this.Images)
			{
				images += item.FileName + " ";
			}
			return $"ProductID: {ProductID}, Name: {Name}, Description: {Description}, UnitPrice: {UnitPrice}, CategoryID: {CategoryID}, Thumbnail: {Thumbnail}, Images: {images}, Quantity: {Quantity}, deleteImages: {deleteImages}";
		}
	}
}
