using Microsoft.EntityFrameworkCore;
using OnlineStore.Domain.Helpers;
using OnlineStore.Domain.Models;

namespace OnlineStore.DataAccess.Repositories
{
	public class OrdersRepository : GenericRepository<Order>
	{
		public OrdersRepository(ApplicationDbContext context) : base(context)
		{

		}
		public async Task<PaginatedDataViewModel<Order>> GetOrderByUserID(int userID, int pageNumber = 1, int pageSize = 5)
		{
			IQueryable<Order> query = _dbContext.Set<Order>();
			query = query
				.Where(o => o.CustomerID == userID)
				.OrderByDescending(o => o.CreatedAt)
				.Include(o => o.OrderDetails)
					.ThenInclude(od => od.Product);

			var data = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync();

			int totalCount = await query.CountAsync();
			return new PaginatedDataViewModel<Order>(data, totalCount);
		}
		public async Task<PaginatedDataViewModel<Order>> GetCancelledOrderByUserID(int userID, int pageNumber = 1, int pageSize = 5)
		{
			IQueryable<Order> query = _dbContext.Set<Order>();
			query = query
				.Where(o => o.CustomerID == userID && o.IsDeleted.Equals(true))
				.OrderByDescending(o => o.CreatedAt)
				.Include(o => o.OrderDetails)
					.ThenInclude(od => od.Product);

			var data = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync();

			int totalCount = await query.CountAsync();
			return new PaginatedDataViewModel<Order>(data, totalCount);
		}
	}
}