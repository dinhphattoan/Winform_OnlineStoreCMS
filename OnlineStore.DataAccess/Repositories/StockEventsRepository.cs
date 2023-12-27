using OnlineStore.Domain.Models;

namespace OnlineStore.DataAccess.Repositories
{
	public class StockEventsRepository : GenericRepository<StockEvent>
	{
		public StockEventsRepository(ApplicationDbContext context) : base(context)
		{

		}
	}
}
