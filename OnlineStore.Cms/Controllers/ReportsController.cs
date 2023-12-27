using Lombok.NET;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.BusinessLogic.Services;
using OnlineStore.Cms.ViewModels.Order;
using OnlineStore.Cms.ViewModels.Product;
using OnlineStore.Domain.DTO.Order;
using OnlineStore.Domain.DTO.Product;
using OnlineStore.Domain.Helpers;
using OnlineStore.Domain.Mapper;

namespace OnlineStore.Cms.Controllers
{
    [RequiredArgsConstructor]
    [Authorize(Roles = "ADMIN,CLERK")]
    public partial class ReportsController : Controller
    {
        private readonly ReportService _reportService;
        private readonly IBaseMapper<ProductDTO, ProductViewModel> _productViewModelMapper;
        private readonly IBaseMapper<OrderDTO, OrderViewModel> _orderViewModelMapper;
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> LowStocks(int? offset, int? limit)
        {
            if (offset == null)
            {
                offset = 0;
            }
            if (limit == null)
            {
                limit = 20;
            }
            int page = (int)Math.Floor((decimal)(offset.Value / limit.Value));
            PaginatedDataViewModel<ProductDTO> productDTOs = await _reportService.LowStockProducts(page + 1, limit.Value, 10);
            IEnumerable<ProductViewModel> productViewModel = _productViewModelMapper.MapList(productDTOs.Data);
            return Json(new { rows = productViewModel, total = productDTOs.TotalCount });
        }

        [HttpGet]
        public async Task<ActionResult> TodayOrders(int? offset, int? limit)
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
            PaginatedDataViewModel<OrderDTO> orderDTOs = await _reportService.TodayOrders(page + 1, limit.Value);
            IEnumerable<OrderViewModel> orderViewModels = _orderViewModelMapper.MapList(orderDTOs.Data);
            return Json(new { rows = orderViewModels, total = orderDTOs.TotalCount });
        }

        [HttpGet]
        public async Task<ActionResult> HighestTotal(int? offset, int? limit)
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
            PaginatedDataViewModel<OrderDTO> orderDTOs = await _reportService.HighestTotal(page + 1, limit.Value);
            IEnumerable<OrderViewModel> orderViewModels = _orderViewModelMapper.MapList(orderDTOs.Data);
            return Json(new { rows = orderViewModels, total = orderDTOs.TotalCount });
        }
    }
}
