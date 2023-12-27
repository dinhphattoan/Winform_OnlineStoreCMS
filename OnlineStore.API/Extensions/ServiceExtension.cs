using AutoMapper;
using OnlineStore.Api.Authentication;
using OnlineStore.Api.ViewModel.Auth;
using OnlineStore.Api.ViewModel.Categories;
using OnlineStore.Api.ViewModel.Orders;
using OnlineStore.Api.ViewModel.Product;
using OnlineStore.Api.ViewModel.User;
using OnlineStore.API.ViewModel.Product;
using OnlineStore.BusinessLogic.Services;
using OnlineStore.BusinessLogic.Services.Auth;
using OnlineStore.BusinessLogic.Services.Interface;
using OnlineStore.DataAccess;
using OnlineStore.DataAccess.Repositories;
using OnlineStore.Domain.DTO.Category;
using OnlineStore.Domain.DTO.Order;
using OnlineStore.Domain.DTO.OrderDetail;
using OnlineStore.Domain.DTO.Product;
using OnlineStore.Domain.DTO.ProductImage;
using OnlineStore.Domain.DTO.Stock;
using OnlineStore.Domain.DTO.StockEvent;
using OnlineStore.Domain.DTO.User;
using OnlineStore.Domain.Mapper;
using OnlineStore.Domain.Models;
namespace OnlineStore.Cms.Extensions
{
	public static class ServiceExtension
	{
		public static IServiceCollection RegisterService(this IServiceCollection services)
		{
			#region Services
			services.AddScoped<ProductsService>();
			services.AddScoped<CategoriesService>();
			services.AddScoped<StocksService>();
			services.AddScoped<StockEventService>();
			services.AddScoped<OrderService>();
			services.AddScoped<ReportService>();
			services.AddScoped<UserService>();
			services.AddScoped<ILoginService, LoginService>();
			services.AddScoped<IRegisterService, RegisterService>();
			services.AddScoped<IAuthentication, JwtAuthentication>();
			services.AddScoped<IUserProvider, UserProvider>();
			#endregion

			#region Repositories
			//services.AddTransient<GenericRepository<Product>, ProductsRepository>();
			//services.AddTransient<GenericRepository<Category>, CategoriesRepository>();
			//services.AddTransient<CloudinaryService>();
			//services.AddTransient<GenericRepository<ProductImage>, ProductImagesRepository>();
			//services.AddTransient<GenericRepository<Stock>, StocksRepository>();
			//services.AddTransient<GenericRepository<StockEvent>, StockEventsRepository>();
			//services.AddTransient<GenericRepository<User>, UsersRepository>();
			//services.AddTransient<GenericRepository<Order>, OrdersRepository>();
			//services.AddTransient<GenericRepository<OrderDetail>, OrderDetailsRepository>();
			//services.AddTransient<IDisposable, UnitOfWork>();
			services.AddTransient<ProductsRepository>();
			services.AddTransient<CategoriesRepository>();
			services.AddTransient<CloudinaryService>();
			services.AddTransient<ProductImagesRepository>();
			services.AddTransient<StocksRepository>();
			services.AddTransient<StockEventsRepository>();
			services.AddTransient<UsersRepository>();
			services.AddTransient<OrdersRepository>();
			services.AddTransient<OrderDetailsRepository>();
			services.AddTransient<UnitOfWork>();
			#endregion

			#region Mapper
			var configuration = new MapperConfiguration(cfg =>
			{
				#region Product
				cfg.CreateMap<Product, ProductDTO>()
					.ForMember(dest => dest.Category, opt => opt.MapFrom(src => new BaseCategoyDTO
					{
						CategoryID = src.Category.CategoryID,
						Name = src.Category.Name
					}))
					.ForMember(dest => dest.ProductImages, opt => opt.MapFrom(src => src.ProductImages.Select(pi => new BaseProductImageDTO
					{
						ProductImageID = pi.ProductImageID,
						Order = pi.Order,
						Path = pi.Path
					})))
					.ForMember(dest => dest.CreateBy, opt => opt.MapFrom(src => new BaseUserDTO
					{
						UserID = src.CreateBy.UserID,
						FirstName = src.CreateBy.FirstName,
						LastName = src.CreateBy.LastName
					}))
					.ForMember(dest => dest.Stock, opt => opt.MapFrom(src => new BaseStockDTO
					{
						StockID = src.Stock.StockID,
						Quantity = src.Stock.Quantity
					}));
				cfg.CreateMap<ProductDTO, Product>()
				.ForMember(dest => dest.Category, opt => opt.Ignore())
				.ForMember(dest => dest.CreateBy, opt => opt.Ignore())
				.ForMember(dest => dest.CategoryID, opt => opt.MapFrom(src => src.Category.CategoryID))
				.ForMember(dest => dest.CreateByID, opt => opt.MapFrom(src => src.CreateBy.UserID))
				.ForMember(dest => dest.ProductImages, opt => opt.MapFrom(src => src.ProductImages.Select(pi => new ProductImage
				{
					ProductImageID = pi.ProductImageID,
					Order = pi.Order,
					Path = pi.Path
				})));

				cfg.CreateMap<ProductDTO, ProductRes>()
					.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductID))
					.ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
					.ForMember(dest => dest.Stock, a => a.MapFrom(p => p.Stock != null ? p.Stock.Quantity : 0))
					.ForMember(d => d.ShortDescription, a => a.MapFrom(p => p.Description != null && p.Description.Length > 50 ? p.Description.Substring(0, 50) + "..." : p.Description));

				cfg.CreateMap<ProductDTO, ProductDetailRes>()
					.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductID))
					.ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
					.ForMember(dest => dest.Stock, a => a.MapFrom(p => p.Stock != null ? p.Stock.Quantity : 0))
					.ForMember(dest => dest.Images, a => a.MapFrom(p => p.ProductImages.Select(pi => pi.Path).ToList()));
				#endregion

				#region Category
				cfg.CreateMap<Category, CategoryDTO>()
					.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Desc));
				cfg.CreateMap<CategoryDTO, Category>()
					.ForMember(dest => dest.Desc, opt => opt.MapFrom(src => src.Description));
				cfg.CreateMap<CategoryDTO, BaseCategory>()
					.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CategoryID));
				#endregion

				#region Stock
				cfg.CreateMap<StockDTO, Stock>()
					.ForMember(dest => dest.Product, opt => opt.Ignore())
					.ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.Product.ProductID));
				cfg.CreateMap<Stock, StockDTO>();
				#endregion

				#region StockEvent
				cfg.CreateMap<StockEventDTO, StockEvent>()
					.ForMember(dest => dest.Stock, opt => opt.Ignore())
					.ForMember(dest => dest.StockID, opt => opt.MapFrom(src => src.Stock.StockID));
				cfg.CreateMap<StockEvent, StockEventDTO>()
					.ForMember(dest => dest.Stock, opt => opt.MapFrom(src => new StockDTO
					{
						StockID = src.Stock.StockID,
						Quantity = src.Stock.Quantity,
						Product = new ProductDTO
						{
							ProductID = src.Stock.Product.ProductID,
							Name = src.Stock.Product.Name
						}
					}));
				#endregion

				#region Order
				cfg.CreateMap<Order, OrderDTO>()
					.ForMember(dest => dest.Clerk, opt => opt.MapFrom(src => src.Clerk != null ? new UserDTO
					{
						UserID = src.Clerk.UserID,
						FirstName = src.Clerk.FirstName,
						LastName = src.Clerk.LastName
					} : null))
					.ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer != null ? new UserDTO
					{
						UserID = src.Customer.UserID,
						FirstName = src.Customer.FirstName,
						LastName = src.Customer.LastName
					} : null))
					.ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src =>
						src.OrderDetails != null ? src.OrderDetails.Select(od => new OrderDetailDTO
						{
							OrderID = od.OrderID,
							OrderDetailID = od.OrderDetailID,
							Quantity = od.Quantity,
							UnitPrice = od.UnitPrice,
							Product = new ProductDTO
							{
								ProductID = od.Product.ProductID,
								Name = od.Product.Name,
								Thumbnail = od.Product.Thumbnail,
								UnitPrice = od.Product.UnitPrice
							},
						}) : null));
				cfg.CreateMap<OrderDTO, Order>()
					.ForMember(dest => dest.Clerk, opt => opt.Ignore())
					.ForMember(dest => dest.Customer, opt => opt.Ignore())
					.ForMember(dest => dest.ClerkID, opt => opt.MapFrom(src => src.Clerk.UserID))
					.ForMember(dest => dest.CustomerID, opt => opt.MapFrom(src => src.Customer.UserID))
					.ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(
						src => src.OrderDetails.Select(oi => new OrderDetail
						{
							OrderDetailID = oi.OrderDetailID,
							ProductID = oi.Product.ProductID,
							Quantity = oi.Quantity,
							UnitPrice = oi.UnitPrice
						}))
						);
				cfg.CreateMap<OrderDTO, OrderRes>()
				.ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.OrderDetails.Sum(od => od.UnitPrice * od.Quantity)))
				.ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(
					src => src.OrderDetails.Select(oi => new OrderDetailRes
					{
						Thumbnail = oi.Product.Thumbnail,
						ProductName = oi.Product.Name,
						Quantity = oi.Quantity,
						UnitPrice = oi.UnitPrice
					})));
				#endregion

				#region OrderDetail
				cfg.CreateMap<OrderDetail, OrderDetailDTO>()
					.ForMember(dest => dest.Product, opt => opt.MapFrom(src => new ProductDTO
					{
						ProductID = src.Product.ProductID,
						Name = src.Product.Name,
						Thumbnail = src.Product.Thumbnail,
						UnitPrice = src.Product.UnitPrice
					}));

				cfg.CreateMap<OrderDetailDTO, OrderDetail>()
					.ForMember(dest => dest.Product, opt => opt.Ignore())
					.ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.Product.ProductID));

				cfg.CreateMap<OrderDetailReq, OrderDetailDTO>()
					.ForMember(dest => dest.Product, opt => opt.MapFrom(src => new ProductDTO
					{
						ProductID = src.ProductID
					}));
				#endregion

				#region User
				cfg.CreateMap<User, UserDTO>();
				cfg.CreateMap<UserDTO, User>();
				cfg.CreateMap<RegisterReq, UserDTO>();
				cfg.CreateMap<UserDTO, BaseUserRes>()
					.ForMember(dest => dest.id, opt => opt.MapFrom(src => src.UserID));

				cfg.CreateMap<UserDTO, UserRes>();
				#endregion
			});

			IMapper mapper = configuration.CreateMapper();

			// product
			services.AddSingleton<IBaseMapper<Product, ProductDTO>>(new
				BaseMapper<Product, ProductDTO>(mapper));

			services.AddSingleton<IBaseMapper<ProductDTO, Product>>(new
				BaseMapper<ProductDTO, Product>(mapper));

			services.AddSingleton<IBaseMapper<ProductDTO, ProductRes>>(new
				BaseMapper<ProductDTO, ProductRes>(mapper));
			services.AddSingleton<IBaseMapper<ProductDTO, ProductDetailRes>>(new
				BaseMapper<ProductDTO, ProductDetailRes>(mapper));
			// category

			services.AddSingleton<IBaseMapper<Category, CategoryDTO>>(new
				BaseMapper<Category, CategoryDTO>(mapper));

			services.AddSingleton<IBaseMapper<CategoryDTO, Category>>(new
				BaseMapper<CategoryDTO, Category>(mapper));
			services.AddSingleton<IBaseMapper<CategoryDTO, BaseCategory>>(new
					BaseMapper<CategoryDTO, BaseCategory>(mapper));
			// stock
			services.AddSingleton<IBaseMapper<StockDTO, Stock>>(new
				BaseMapper<StockDTO, Stock>(mapper));

			services.AddSingleton<IBaseMapper<Stock, StockDTO>>(new
				BaseMapper<Stock, StockDTO>(mapper));

			services.AddSingleton<IBaseMapper<StockEvent, StockEventDTO>>(new
				BaseMapper<StockEvent, StockEventDTO>(mapper));

			services.AddSingleton<IBaseMapper<StockEventDTO, StockEvent>>(new
				BaseMapper<StockEventDTO, StockEvent>(mapper));

			// order
			services.AddSingleton<IBaseMapper<Order, OrderDTO>>(new
				BaseMapper<Order, OrderDTO>(mapper));

			services.AddSingleton<IBaseMapper<OrderDTO, Order>>(new
				BaseMapper<OrderDTO, Order>(mapper));

			services.AddSingleton<IBaseMapper<OrderDTO, OrderRes>>(new
				BaseMapper<OrderDTO, OrderRes>(mapper));

			services.AddSingleton<IBaseMapper<OrderDetail, OrderDetailDTO>>(new
				BaseMapper<OrderDetail, OrderDetailDTO>(mapper));

			services.AddSingleton<IBaseMapper<OrderDetailDTO, OrderDetail>>(new
				BaseMapper<OrderDetailDTO, OrderDetail>(mapper));

			services.AddSingleton<IBaseMapper<OrderDetailReq, OrderDetailDTO>>(new
				BaseMapper<OrderDetailReq, OrderDetailDTO>(mapper));

			// user

			services.AddSingleton<IBaseMapper<User, UserDTO>>(new
					BaseMapper<User, UserDTO>(mapper));
			services.AddSingleton<IBaseMapper<UserDTO, User>>(new
					BaseMapper<UserDTO, User>(mapper));
			services.AddSingleton<IBaseMapper<RegisterReq, UserDTO>>(new
					BaseMapper<RegisterReq, UserDTO>(mapper));
			services.AddSingleton<IBaseMapper<UserDTO, BaseUserRes>>(new
					BaseMapper<UserDTO, BaseUserRes>(mapper));
			services.AddSingleton<IBaseMapper<UserDTO, UserRes>>(new
					BaseMapper<UserDTO, UserRes>(mapper));
			#endregion

			return services;
		}
	}
}
