using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using OnlineStore.BusinessLogic.Services;
using OnlineStore.Cms.ViewModels.Categories;
using OnlineStore.Cms.ViewModels.Order;
using OnlineStore.Cms.ViewModels.OrderDetail;
using OnlineStore.Cms.ViewModels.Product;
using OnlineStore.Cms.ViewModels.Stock;
using OnlineStore.Cms.ViewModels.StockEvent;
using OnlineStore.Cms.ViewModels.Users;
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
			#region Authentication
			services.AddAuthentication(options =>
			{
				options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
				options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			})
			.AddCookie(options =>
			{
				options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
				options.LoginPath = "/login";
				options.LogoutPath = "/logout";
				options.AccessDeniedPath = "/AccessDenied";
			});
			#endregion
			#region Services
			services.AddScoped<ProductsService>();
			services.AddScoped<CategoriesService>();
			services.AddScoped<StocksService>();
			services.AddScoped<StockEventService>();
			services.AddScoped<OrderService>();
			services.AddScoped<ReportService>();
			services.AddScoped<UserService>();
			services.AddScoped<LoginService>();
			#endregion

			#region Repositories
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
				cfg.CreateMap<ProductDTO, ProductViewModel>()
				.ForMember(d => d.UnitPrice, a => a.MapFrom(p => p.UnitPrice != null ? "$" + p.UnitPrice.ToString("N2") : "$0.00"))
				.ForMember(d => d.ShortDescription, a => a.MapFrom(p => p.Description != null && p.Description.Length > 100 ? p.Description.Substring(0, 100) + "..." : p.Description))
				.ForMember(d => d.createByName, a => a.MapFrom(p => p.CreateBy != null ? p.CreateBy.FirstName + " " + p.CreateBy.LastName : ""))
				.ForMember(d => d.Quantity, a => a.MapFrom(p => p.Stock != null ? p.Stock.Quantity : 0))
				.ForMember(d => d.CategoryName, o => o.MapFrom(p => p.Category.Name))
				.ForMember(d => d.CategoryID, o => o.MapFrom(p => p.Category.CategoryID))
				.ForMember(d => d.ProductImages, o => o.MapFrom(p => p.ProductImages.Select(pi => new ProductImageViewModel
				{
					ProductImageID = pi.ProductImageID,
					Order = pi.Order,
					Path = pi.Path
				})));

				cfg.CreateMap<ProductReqViewModel, ProductDTO>()
				.ForMember(d => d.UnitPrice, o => o.MapFrom(p => p.UnitPrice != null ? p.UnitPrice : 0))
				.ForMember(d => d.Description, o => o.MapFrom(p => p.Description != null ? p.Description : ""))
				.ForMember(d => d.Name, o => o.MapFrom(p => p.Name != null ? p.Name : ""))
				.ForMember(d => d.Category, o => o.MapFrom(src => new
				BaseCategoyDTO
				{
					CategoryID = src.CategoryID.Value
				}));

				cfg.CreateMap<ProductDTO, ProductReqViewModel>()
					.ForMember(d => d.UnitPrice, o => o.MapFrom(p => p.UnitPrice != null ? p.UnitPrice : 0))
					.ForMember(d => d.Description, o => o.MapFrom(p => p.Description != null ? p.Description : ""))
					.ForMember(d => d.Name, o => o.MapFrom(p => p.Name != null ? p.Name : ""))
					.ForMember(d => d.CategoryID, o => o.MapFrom(src => src.Category.CategoryID));

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
				#endregion

				#region Category
				cfg.CreateMap<Category, CategoryDTO>()
					.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Desc));
				cfg.CreateMap<CategoryDTO, Category>()
					.ForMember(dest => dest.Desc, opt => opt.MapFrom(src => src.Description));

				cfg.CreateMap<CategoryDTO, CategoriesListViewModel>()
					.ForMember(d => d.ShortDescription, o => o.MapFrom(c => c.Description != null && c.Description.Length > 80 ? c.Description.Substring(0, 80) + "..." : c.Description));

				cfg.CreateMap<CategoriesReqViewModel, CategoryDTO>()
					.ForMember(dest => dest.Image, opt => opt.Ignore());

				cfg.CreateMap<CategoryDTO, CategoriesViewModel>()
					.ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image != null ? src.Image : ""));

				#endregion

				#region Stock
				cfg.CreateMap<StockDTO, Stock>()
					.ForMember(dest => dest.Product, opt => opt.Ignore())
					.ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.Product.ProductID));
				cfg.CreateMap<Stock, StockDTO>();

				cfg.CreateMap<StockDTO, StockListViewModel>()
					.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name))
					.ForMember(dest => dest.Thumbnail, opt => opt.MapFrom(src => src.Product.Thumbnail))
					.ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Product.Category.Name));

				cfg.CreateMap<StockDTO, BaseStockViewModel>()
					.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name));
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
				cfg.CreateMap<StockEventDTO, StockEventViewModel>()
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Stock.Product.Name))
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss")));
				#endregion

				#region Order
				cfg.CreateMap<Order, OrderDTO>()
					.ForMember(dest => dest.Clerk, opt => opt.MapFrom(src => new UserDTO
					{
						UserID = src.Clerk.UserID,
						FirstName = src.Clerk.FirstName,
						LastName = src.Clerk.LastName
					}))
					.ForMember(dest => dest.Customer, opt => opt.MapFrom(src => new UserDTO
					{
						UserID = src.Customer.UserID,
						FirstName = src.Customer.FirstName,
						LastName = src.Customer.LastName
					}));
				cfg.CreateMap<OrderDTO, Order>()
					.ForMember(dest => dest.Clerk, opt => opt.Ignore())
					.ForMember(dest => dest.Customer, opt => opt.Ignore())
					.ForMember(dest => dest.ClerkID, opt => opt.MapFrom(src => src.Clerk.UserID))
					.ForMember(dest => dest.CustomerID, opt => opt.MapFrom(src => src.Customer.UserID));

				cfg.CreateMap<OrderDTO, OrderViewModel>()
					.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToString()))
					.ForMember(dest => dest.ClerkName, opt => opt.MapFrom(src => src.Clerk.FirstName + " " + src.Clerk.LastName))
					.ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.FirstName + " " + src.Customer.LastName))
					.ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.OrderDetails.Sum(oi => oi.UnitPrice * oi.Quantity)))
					.ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderDetails.Select(oi => new OrderDetailViewModel
					{
						OrderDetailID = oi.OrderDetailID,
						Quantity = oi.Quantity,
						UnitPrice = oi.UnitPrice,
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

				cfg.CreateMap<OrderDetailDTO, OrderDetailViewModel>()
					.ForMember(dest => dest.Product, opt => opt.MapFrom(src => new BaseProductViewModel
					{
						ProductID = src.Product.ProductID,
						Name = src.Product.Name,
						Thumbnail = src.Product.Thumbnail
					}))
					.ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Product.UnitPrice));
				#endregion

				#region User
				cfg.CreateMap<User, UserDTO>();
				cfg.CreateMap<UserDTO, User>();
				cfg.CreateMap<UserDTO, UserListViewModel>()
					.ForMember(dest => dest.Role, oct => oct.MapFrom(src => src.Role.ToString()));
				cfg.CreateMap<UserReqViewModel, UserDTO>();

				cfg.CreateMap<UserDTO, UserViewModel>()
					.ForMember(dest => dest.DateOfBirth, oct => oct.MapFrom(src => src.DateOfBirth.ToString("yyyy-MM-dd")));

				cfg.CreateMap<UserUpdateViewModel, UserDTO>()
					.ForMember(dest => dest.Password, oct => oct.MapFrom(src => src.NewPassword));
				#endregion
			});

			IMapper mapper = configuration.CreateMapper();

			// product
			services.AddSingleton<IBaseMapper<Product, ProductDTO>>(new
				BaseMapper<Product, ProductDTO>(mapper));

			services.AddSingleton<IBaseMapper<ProductDTO, Product>>(new
				BaseMapper<ProductDTO, Product>(mapper));

			services.AddSingleton<IBaseMapper<ProductDTO, ProductViewModel>>(new
				BaseMapper<ProductDTO, ProductViewModel>(mapper));

			services.AddSingleton<IBaseMapper<ProductReqViewModel, ProductDTO>>(new
				BaseMapper<ProductReqViewModel, ProductDTO>(mapper));

			services.AddSingleton<IBaseMapper<ProductDTO, ProductReqViewModel>>(new
				BaseMapper<ProductDTO, ProductReqViewModel>(mapper));

			// category

			services.AddSingleton<IBaseMapper<Category, CategoryDTO>>(new
				BaseMapper<Category, CategoryDTO>(mapper));

			services.AddSingleton<IBaseMapper<CategoryDTO, Category>>(new
				BaseMapper<CategoryDTO, Category>(mapper));

			services.AddSingleton<IBaseMapper<CategoryDTO, CategoriesListViewModel>>(new
				BaseMapper<CategoryDTO, CategoriesListViewModel>(mapper));

			services.AddSingleton<IBaseMapper<CategoriesReqViewModel, CategoryDTO>>(new
				BaseMapper<CategoriesReqViewModel, CategoryDTO>(mapper));

			services.AddSingleton<IBaseMapper<CategoryDTO, CategoriesViewModel>>(new
				BaseMapper<CategoryDTO, CategoriesViewModel>(mapper));

			// stock
			services.AddSingleton<IBaseMapper<StockDTO, Stock>>(new
				BaseMapper<StockDTO, Stock>(mapper));

			services.AddSingleton<IBaseMapper<Stock, StockDTO>>(new
				BaseMapper<Stock, StockDTO>(mapper));

			services.AddSingleton<IBaseMapper<StockDTO, StockListViewModel>>(new
				BaseMapper<StockDTO, StockListViewModel>(mapper));

			services.AddSingleton<IBaseMapper<StockEvent, StockEventDTO>>(new
				BaseMapper<StockEvent, StockEventDTO>(mapper));

			services.AddSingleton<IBaseMapper<StockEventDTO, StockEvent>>(new
				BaseMapper<StockEventDTO, StockEvent>(mapper));

			services.AddSingleton<IBaseMapper<StockDTO, BaseStockViewModel>>(new
				BaseMapper<StockDTO, BaseStockViewModel>(mapper));

			services.AddSingleton<IBaseMapper<StockEventDTO, StockEventViewModel>>(new
				BaseMapper<StockEventDTO, StockEventViewModel>(mapper));

			// order
			services.AddSingleton<IBaseMapper<Order, OrderDTO>>(new
				BaseMapper<Order, OrderDTO>(mapper));

			services.AddSingleton<IBaseMapper<OrderDTO, Order>>(new
				BaseMapper<OrderDTO, Order>(mapper));

			services.AddSingleton<IBaseMapper<OrderDTO, OrderViewModel>>(new
				BaseMapper<OrderDTO, OrderViewModel>(mapper));

			services.AddSingleton<IBaseMapper<OrderDetail, OrderDetailDTO>>(new
				BaseMapper<OrderDetail, OrderDetailDTO>(mapper));

			services.AddSingleton<IBaseMapper<OrderDetailDTO, OrderDetail>>(new
				BaseMapper<OrderDetailDTO, OrderDetail>(mapper));

			services.AddSingleton<IBaseMapper<OrderDetailDTO, OrderDetailViewModel>>(new
				BaseMapper<OrderDetailDTO, OrderDetailViewModel>(mapper));

			// user

			services.AddSingleton<IBaseMapper<User, UserDTO>>(new
					BaseMapper<User, UserDTO>(mapper));
			services.AddSingleton<IBaseMapper<UserDTO, User>>(new
					BaseMapper<UserDTO, User>(mapper));
			services.AddSingleton<IBaseMapper<UserDTO, UserListViewModel>>(new
					BaseMapper<UserDTO, UserListViewModel>(mapper));
			services.AddSingleton<IBaseMapper<UserReqViewModel, UserDTO>>(new
					BaseMapper<UserReqViewModel, UserDTO>(mapper));
			services.AddSingleton<IBaseMapper<UserDTO, UserViewModel>>(new
					BaseMapper<UserDTO, UserViewModel>(mapper));
			services.AddSingleton<IBaseMapper<UserUpdateViewModel, UserDTO>>(new
					BaseMapper<UserUpdateViewModel, UserDTO>(mapper));
			#endregion

			return services;
		}
	}
}
