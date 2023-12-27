using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace OnlineStore.Cms.ViewModels.Categories
{
	public class CategoriesReqViewModel : BaseCategoriesViewModel
	{
		[Required]
		public string Description { get; set; }

		[AllowNull]
		public IFormFile? Image { get; set; }
	}
}
