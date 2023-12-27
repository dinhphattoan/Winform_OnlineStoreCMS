using Lombok.NET;
using OnlineStore.DataAccess;
using OnlineStore.Domain.DTO.Order;
using OnlineStore.Domain.Enums;
using OnlineStore.Domain.Exceptions;
using OnlineStore.Domain.Helpers;
using OnlineStore.Domain.Mapper;
using OnlineStore.Domain.Models;
using System.Linq.Expressions;
using System.Net;

namespace OnlineStore.BusinessLogic.Services
{
	[RequiredArgsConstructor]
	public partial class OrderService
	{
		private readonly UnitOfWork _unitOfWork;
		private readonly IBaseMapper<Order, OrderDTO> _orderDTOMapper;
		private readonly IBaseMapper<OrderDTO, Order> _orderMapper;
		public async Task<PaginatedDataViewModel<OrderDTO>> GetPaginate(int PageNumber, int PageSize, string? search)
		{
			Expression<Func<Order, object>>[] include =
			{
				o => o.Customer,
				o => o.Clerk,
				// TODO: Include OrderItems
			};
			Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy = q => q.OrderByDescending(c => c.CreatedAt);
			List<Expression<Func<Order, bool>>> filters = new List<Expression<Func<Order, bool>>>();
			if (search != null)
			{
				filters.Add(o => o.Customer.FirstName.ToLower().Contains(search) || o.Customer.LastName.ToLower().Contains(search) || (o.Customer.FirstName + " " + o.Customer.LastName).ToLower().Contains(search));
			}
			PaginatedDataViewModel<Order> paginatedDataViewModel = await _unitOfWork.OrderRepository.GetPaginatedData(filters, orderBy, include, PageNumber, PageSize);
			IEnumerable<Order> orders = paginatedDataViewModel.Data;
			IEnumerable<OrderDTO> orderDTOs = _orderDTOMapper.MapList(orders);
			PaginatedDataViewModel<OrderDTO> new_paginatedDataViewModel = new PaginatedDataViewModel<OrderDTO>(orderDTOs, paginatedDataViewModel.TotalCount);
			return new_paginatedDataViewModel;
		}
		private async Task<Product> UpdateStock(Product product, int quantity, int userID)
		{
			List<Expression<Func<Stock, bool>>> filters = new List<Expression<Func<Stock, bool>>> { p => p.ProductID.Equals(product.ProductID) };
			Stock stock = _unitOfWork.StockRepository.Get(filters: filters).Result.FirstOrDefault();
			if (stock == null) throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Stock not found");
			StockEvent stockEvent = new StockEvent
			{
				Stock = stock,
				Quantity = quantity,
				Type = StockEventTypes.OUT,
				Reason = $"Sell to the user with ID:{userID}",
				CreatedAt = DateTimeOffset.Now
			};
			stock.Quantity -= quantity;
			try
			{
				_unitOfWork.StockEventRepository.Create(stockEvent);
				_unitOfWork.StockRepository.Update(stock);
			}
			catch (Exception e)
			{
				throw new HttpStatusCodeException(HttpStatusCode.BadGateway, $"Error while updating stock, {e}");
			}
			return product;
		}
		public async Task<OrderDTO> Create(OrderDTO orderDTO)
		{
			Order order = _orderMapper.MapModel(orderDTO);
			try
			{
				Order new_order = await _unitOfWork.OrderRepository.Create(order);
				new_order.CreatedAt = DateTimeOffset.Now;
				if (new_order.OrderDetails == null || new_order.OrderDetails.Count <= 0) throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "Order details are required");

				foreach (var od in new_order.OrderDetails)
				{
					try
					{
						Product tmp = await _unitOfWork.ProductRepository.GetById(od.ProductID);
						tmp = await UpdateStock(tmp, od.Quantity, new_order.CustomerID);
						od.UnitPrice = tmp.UnitPrice;
						od.Order = new_order;
					}
					catch (NotFoundException e)
					{
						throw new HttpStatusCodeException(HttpStatusCode.NotFound, e);
					}
				}

				_unitOfWork.Save();
				return orderDTO;
			}
			catch (Exception e)
			{
				throw new HttpStatusCodeException(HttpStatusCode.BadGateway, $"Error while creating order, {e}");
			}

		}

		public async Task<PaginatedDataViewModel<OrderDTO>> GetOrderByUserID(int userID, int page, int size)
		{
			try
			{
				PaginatedDataViewModel<Order> paginatedDataViewModel = await _unitOfWork.OrderRepository.GetOrderByUserID(userID, page, size);
				IEnumerable<Order> orders = paginatedDataViewModel.Data;
				IEnumerable<OrderDTO> orderDTOs = _orderDTOMapper.MapList(orders);
				PaginatedDataViewModel<OrderDTO> new_paginatedDataViewModel = new PaginatedDataViewModel<OrderDTO>(orderDTOs, paginatedDataViewModel.TotalCount);
				return new_paginatedDataViewModel;
			}
			catch (NotFoundException e)
			{
				throw new HttpStatusCodeException(HttpStatusCode.NotFound, e);
			}
			catch (Exception e)
			{
				throw new HttpStatusCodeException(HttpStatusCode.BadGateway, $"Error while getting orders, {e}");
			}
		}
		public async Task<PaginatedDataViewModel<OrderDTO>> GetCancelledOrderByUserID(int userID, int page, int size)
		{
			try
			{
				PaginatedDataViewModel<Order> paginatedDataViewModel = await _unitOfWork.OrderRepository.GetCancelledOrderByUserID(userID, page, size);
				IEnumerable<Order> orders = paginatedDataViewModel.Data;
				IEnumerable<OrderDTO> orderDTOs = _orderDTOMapper.MapList(orders);
				PaginatedDataViewModel<OrderDTO> new_paginatedDataViewModel = new PaginatedDataViewModel<OrderDTO>(orderDTOs, paginatedDataViewModel.TotalCount);
				return new_paginatedDataViewModel;
			}
			catch (NotFoundException e)
			{
				throw new HttpStatusCodeException(HttpStatusCode.NotFound, e);
			}
			catch (Exception e)
			{
				throw new HttpStatusCodeException(HttpStatusCode.BadGateway, $"Error while getting orders, {e}");
			}
		}
	}
}
