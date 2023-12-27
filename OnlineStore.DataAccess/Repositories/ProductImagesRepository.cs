using OnlineStore.Domain.Constants;
using OnlineStore.Domain.Models;

namespace OnlineStore.DataAccess.Repositories
{
	public class ProductImagesRepository : GenericRepository<ProductImage>
	{
		public ProductImagesRepository(ApplicationDbContext context) : base(context)
		{

		}

		// delete by id
		public async Task<string> DeleteById(int id)
		{
			ProductImage productImages = await _dbContext.FindAsync<ProductImage>(id);
			_dbContext.Remove(productImages);
			return SuccessMessages.DELETE_SUCCESS;
		}
	}
}
