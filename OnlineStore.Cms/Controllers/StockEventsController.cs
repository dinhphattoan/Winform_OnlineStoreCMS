using Lombok.NET;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.BusinessLogic.Services;
using OnlineStore.Cms.ViewModels.StockEvent;
using OnlineStore.Domain.DTO.StockEvent;
using OnlineStore.Domain.Helpers;
using OnlineStore.Domain.Mapper;

namespace OnlineStore.Cms.Controllers
{
    [RequiredArgsConstructor]
    [Authorize(Roles = "ADMIN,CLERK")]
    public partial class StockEventsController : Controller
    {
        private readonly StockEventService _stockEventService;
        private readonly IBaseMapper<StockEventDTO, StockEventViewModel> _stockEventViewModelMapper;
        // GET: StocksController
        public ActionResult Index()
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
            PaginatedDataViewModel<StockEventDTO> stocks = await _stockEventService.GetStocksPaginate(page + 1, limit.Value, search);
            IEnumerable<StockEventViewModel> stocksListViewModel = _stockEventViewModelMapper.MapList(stocks.Data);
            return Json(new { rows = stocksListViewModel, total = stocks.TotalCount });
        }

    }
}
