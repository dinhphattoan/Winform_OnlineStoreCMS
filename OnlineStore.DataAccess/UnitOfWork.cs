using OnlineStore.DataAccess.Repositories;

namespace OnlineStore.DataAccess
{
	public class UnitOfWork : IDisposable
	{
		private readonly ApplicationDbContext context;
		private CategoriesRepository categoryRepository;
		private ProductsRepository productRepository;
		private StocksRepository stockRepository;
		private StockEventsRepository stockEventRepository;
		private OrdersRepository orderRepository;
		private OrderDetailsRepository orderDetailRepository;
		private ProductImagesRepository productImageRepository;
		private UsersRepository userRepository;

		public UnitOfWork(ApplicationDbContext dbContext)
		{
			context = dbContext;
		}

		// Kiểm tra xem repository đã được khởi tạo chưa
		public ProductsRepository ProductRepository
		{
			get
			{
				if (this.productRepository == null)
				{
					this.productRepository = new ProductsRepository(context);
				}
				return productRepository;
			}
		}
		public CategoriesRepository CategoriesRepository
		{
			get
			{
				if (this.categoryRepository == null)
				{
					this.categoryRepository = new CategoriesRepository(context);
				}
				return categoryRepository;
			}
		}
		public ProductImagesRepository ProductImageRepository
		{
			get
			{
				if (this.productImageRepository == null)
				{
					this.productImageRepository = new ProductImagesRepository(context);
				}
				return productImageRepository;
			}
		}

		public StockEventsRepository StockEventRepository
		{
			get
			{
				if (this.stockEventRepository == null)
				{
					this.stockEventRepository = new StockEventsRepository(context);
				}
				return stockEventRepository;
			}
		}

		public StocksRepository StockRepository
		{
			get
			{
				if (this.stockRepository == null)
				{
					this.stockRepository = new StocksRepository(context);
				}
				return stockRepository;
			}
		}

		public OrdersRepository OrderRepository
		{
			get
			{
				if (this.orderRepository == null)
				{
					this.orderRepository = new OrdersRepository(context);
				}
				return orderRepository;
			}
		}

		public OrderDetailsRepository OrderDetailRepository
		{
			get
			{
				if (this.orderDetailRepository == null)
				{
					this.orderDetailRepository = new OrderDetailsRepository(context);
				}
				return orderDetailRepository;
			}
		}

		public UsersRepository UserRepository
		{
			get
			{
				if (this.userRepository == null)
				{
					this.userRepository = new UsersRepository(context);
				}
				return userRepository;
			}
		}
		public void Save()
		{
			context.SaveChanges();
		}

		public void SaveAsync()
		{
			context.SaveChangesAsync();
		}

		private bool disposed = false;

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					context.Dispose();
				}
			}
			this.disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
