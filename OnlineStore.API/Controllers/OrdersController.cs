using Lombok.NET;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Api.Authentication;
using OnlineStore.Api.ViewModel.Orders;
using OnlineStore.BusinessLogic.Services;
using OnlineStore.Domain.DTO.Order;
using OnlineStore.Domain.DTO.OrderDetail;
using OnlineStore.Domain.DTO.User;
using OnlineStore.Domain.Exceptions;
using OnlineStore.Domain.Helpers;
using OnlineStore.Domain.Mapper;
using System.Net;

namespace OnlineStore.Api.Controllers
{
	[Route("api/orders")]
	[ApiController]
	[RequiredArgsConstructor]
	public partial class OrdersController : ControllerBase
	{
		private readonly IUserProvider _userProvider;
		private readonly OrderService _orderService;
		private readonly IBaseMapper<OrderDetailReq, OrderDetailDTO> _orderDetailMapper;
		private readonly IBaseMapper<OrderDTO, OrderRes> _orderResMapper;
		[HttpPost]
		[Route("create")]
		public async Task<IActionResult> Create([FromBody] List<OrderDetailReq> orderDetails)
		{
			try
			{
				int userId = _userProvider.GetUserId();
				List<OrderDetailDTO> orderDetailDTOs = _orderDetailMapper.MapList(orderDetails).ToList();
				OrderDTO orderDTO = new OrderDTO
				{
					Customer = new UserDTO() { UserID = userId },
					OrderDetails = orderDetailDTOs
				};
				var order = await _orderService.Create(orderDTO);
				return Ok(order);
			}
			catch (NotFoundException e)
			{
				throw new HttpStatusCodeException(HttpStatusCode.BadGateway, e);
			}
		}

		[HttpGet]
		public async Task<IActionResult> Get(int page, int size)
		{
			try
			{
				int userId = _userProvider.GetUserId();
				PaginatedDataViewModel<OrderDTO> orders = await _orderService.GetOrderByUserID(userId, page, size);
				PaginatedDataViewModel<OrderRes> orderRes = new PaginatedDataViewModel<OrderRes>(_orderResMapper.MapList(orders.Data), orders.TotalCount);
				return Ok(orderRes);
			}
			catch (NotFoundException e)
			{
				throw new HttpStatusCodeException(HttpStatusCode.BadGateway, e);
			}
		}

		[HttpGet]
		[Route("cancel")]
		public async Task<IActionResult> GetCancelled(int page, int size)
		{
			try
			{
				int userId = _userProvider.GetUserId();
				PaginatedDataViewModel<OrderDTO> orders = await _orderService.GetCancelledOrderByUserID(userId, page, size);
				PaginatedDataViewModel<OrderRes> orderRes = new PaginatedDataViewModel<OrderRes>(_orderResMapper.MapList(orders.Data), orders.TotalCount);
				return Ok(orderRes);
			}
			catch (NotFoundException e)
			{
				throw new HttpStatusCodeException(HttpStatusCode.BadGateway, e);
			}
		}
	}
}
