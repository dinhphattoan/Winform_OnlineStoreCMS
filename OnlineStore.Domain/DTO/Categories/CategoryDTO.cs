namespace OnlineStore.Domain.DTO.Category
{
	public class CategoryDTO : BaseCategoyDTO
	{
		public string Description { get; set; }

		public string? Image { get; set; }

		public bool IsDeleted { get; set; }
	}
}
