using Lombok.NET;
using OnlineStore.DataAccess.Repositories;
using OnlineStore.Domain.DTO.Order;
using OnlineStore.Domain.DTO.Product;
using OnlineStore.Domain.Helpers;
using OnlineStore.Domain.Mapper;
using OnlineStore.Domain.Models;
using System.Linq.Expressions;

namespace OnlineStore.BusinessLogic.Services
{
	[RequiredArgsConstructor]
	public partial class ReportService
	{
		private readonly IBaseMapper<Product, ProductDTO> _productDTOMapper;
		private readonly IBaseMapper<Order, OrderDTO> _orderDTOMapper;

		private readonly ProductsRepository _productRepository;
		private readonly OrdersRepository _orderRepository;

		public async Task<PaginatedDataViewModel<ProductDTO>> LowStockProducts(int pageNumber, int pageSize, int quantity)
		{
			try
			{
				List<Expression<Func<Product, bool>>> filters = new List<Expression<Func<Product, bool>>>() { p => p.Stock.Quantity < quantity };
				Expression<Func<Product, object>>[] include =
				{
					p => p.Stock
				};
				PaginatedDataViewModel<Product> paginatedDataViewModel = await _productRepository.GetPaginatedData(filters, null, include, pageNumber, pageSize);
				IEnumerable<Product> products = paginatedDataViewModel.Data;
				IEnumerable<ProductDTO> productDTOs = _productDTOMapper.MapList(products);
				PaginatedDataViewModel<ProductDTO> new_paginatedDataViewModel = new PaginatedDataViewModel<ProductDTO>(productDTOs, paginatedDataViewModel.TotalCount);
				return new_paginatedDataViewModel;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<PaginatedDataViewModel<OrderDTO>> TodayOrders(int pageNumber, int pageSize)
		{
			try
			{
				List<Expression<Func<Order, bool>>> filters = new List<Expression<Func<Order, bool>>>()
				{
					o => o.CreatedAt.Date == DateTime.Now.Date
				};
				Expression<Func<Order, object>>[] include =
				{
					o => o.Customer,
					o => o.Clerk,
				};
				Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy = q => q.OrderByDescending(c => c.CreatedAt);
				PaginatedDataViewModel<Order> paginatedDataViewModel = await _orderRepository.GetPaginatedData(filters, orderBy, include, pageNumber, pageSize);
				IEnumerable<Order> orders = paginatedDataViewModel.Data;
				IEnumerable<OrderDTO> orderDTOs = _orderDTOMapper.MapList(orders);
				PaginatedDataViewModel<OrderDTO> new_paginatedDataViewModel = new PaginatedDataViewModel<OrderDTO>(orderDTOs, paginatedDataViewModel.TotalCount);
				return new_paginatedDataViewModel;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<PaginatedDataViewModel<OrderDTO>> HighestTotal(int pageNumber, int pageSize)
		{
			try
			{
				Expression<Func<Order, object>>[] include =
				{
					o => o.Customer,
					o => o.Clerk,
				};
				//Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy = q => q.OrderByDescending(o => o.Total);
				PaginatedDataViewModel<Order> paginatedDataViewModel = await _orderRepository.GetPaginatedData(null, null, include, pageNumber, pageSize);
				IEnumerable<Order> orders = paginatedDataViewModel.Data;
				IEnumerable<OrderDTO> orderDTOs = _orderDTOMapper.MapList(orders);
				PaginatedDataViewModel<OrderDTO> new_paginatedDataViewModel = new PaginatedDataViewModel<OrderDTO>(orderDTOs, paginatedDataViewModel.TotalCount);
				return new_paginatedDataViewModel;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

	}
}
