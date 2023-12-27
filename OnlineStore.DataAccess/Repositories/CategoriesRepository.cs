using Microsoft.EntityFrameworkCore;
using OnlineStore.Domain.Models;

namespace OnlineStore.DataAccess.Repositories
{
	public class CategoriesRepository : GenericRepository<Category>
	{

		public CategoriesRepository(ApplicationDbContext context) : base(context)
		{

		}
		public async Task<IEnumerable<Category>> GetAllActiveCategories()
		{
			return await _dbContext.Categories.Where(c => c.IsDeleted != true).ToListAsync();
		}
	}
}
