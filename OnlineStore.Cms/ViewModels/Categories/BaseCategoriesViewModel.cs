using Microsoft.Build.Framework;

namespace OnlineStore.Cms.ViewModels.Categories
{
	public class BaseCategoriesViewModel
	{
		public int? CategoryID { get; set; }

		[Required]
		public string Name { get; set; }
	}
}
