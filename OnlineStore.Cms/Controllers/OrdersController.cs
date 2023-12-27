using Lombok.NET;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.BusinessLogic.Services;
using OnlineStore.Cms.ViewModels.Order;
using OnlineStore.Domain.DTO.Order;
using OnlineStore.Domain.Helpers;
using OnlineStore.Domain.Mapper;

namespace OnlineStore.Cms.Controllers
{
	[RequiredArgsConstructor]
	[Authorize(Roles = "ADMIN,CLERK")]
	public partial class OrdersController : Controller
	{
		private readonly OrderService _orderService;
		private readonly IBaseMapper<OrderDTO, OrderViewModel> _orderViewModelMapper;
		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public async Task<ActionResult> GetAll(int? offset, int? limit, string? search)
		{
			if (offset == null)
			{
				offset = 0;
			}
			if (limit == null)
			{
				limit = 10;
			}
			int page = (int)Math.Floor((decimal)(offset.Value / limit.Value));
			PaginatedDataViewModel<OrderDTO> orderDTOs = await _orderService.GetPaginate(page + 1, limit.Value, search);
			IEnumerable<OrderViewModel> orderViewModelData = _orderViewModelMapper.MapList(orderDTOs.Data);
			return Json(new { rows = orderViewModelData, total = orderDTOs.TotalCount });
		}
	}
}
