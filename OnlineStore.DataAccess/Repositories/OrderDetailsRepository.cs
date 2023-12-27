using OnlineStore.Domain.Models;

namespace OnlineStore.DataAccess.Repositories
{
	public class OrderDetailsRepository : GenericRepository<OrderDetail>
	{
		public OrderDetailsRepository(ApplicationDbContext context) : base(context)
		{

		}
	}
}
