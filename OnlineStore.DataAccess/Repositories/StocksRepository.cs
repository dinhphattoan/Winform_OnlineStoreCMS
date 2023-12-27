using Microsoft.EntityFrameworkCore;
using OnlineStore.Domain.Exceptions;
using OnlineStore.Domain.Models;

namespace OnlineStore.DataAccess.Repositories
{
	public class StocksRepository : GenericRepository<Stock>
	{
		public StocksRepository(ApplicationDbContext context) : base(context)
		{

		}

		public async Task<Stock> GetById(int id)
		{
			var data = await _dbContext.Set<Stock>().Where(s => s.StockID.Equals(id))
				.Include(s => s.Product).FirstAsync();
			if (data == null)
			{
				Console.WriteLine("No data found");
				throw new NotFoundException("No data found");
			}

			return data;
		}
	}
}
