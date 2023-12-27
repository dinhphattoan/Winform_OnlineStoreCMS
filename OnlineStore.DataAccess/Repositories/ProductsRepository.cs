using Microsoft.EntityFrameworkCore;
using OnlineStore.Domain.Exceptions;
using OnlineStore.Domain.Models;

namespace OnlineStore.DataAccess.Repositories
{
	public class ProductsRepository : GenericRepository<Product>
	{
		public ProductsRepository(ApplicationDbContext context) : base(context)
		{

		}
		public async Task<Product> GetById(int id)
		{
			var data = await _dbContext.Set<Product>().Where(p => p.ProductID.Equals(id))
				.Include(p => p.Stock)
				.Include(p => p.Category)
				.Include(p => p.ProductImages).FirstAsync();
			if (data == null)
				throw new NotFoundException("No data found");
			return data;
		}

		public async Task<bool> CategoryHasProducts(int id)
		{
			bool result = await _dbContext.Set<Product>()
				.Where(p => p.CategoryID.Equals(id)).AnyAsync();
			return result;
		}
		public async Task<Product> GetByIdForClient(int id)
		{
			var data = await _dbContext.Set<Product>().Where(p => p.ProductID.Equals(id))
				.Include(p => p.Stock)
				.Include(p => p.Category)
				.Where(p => p.IsDeleted.Equals(false))
				.Include(p => p.ProductImages).FirstAsync();

			if (data == null)
				throw new NotFoundException("No data found");
			return data;
		}
	}
}
