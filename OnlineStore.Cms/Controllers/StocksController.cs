using Lombok.NET;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.BusinessLogic.Services;
using OnlineStore.Cms.ViewModels.Stock;
using OnlineStore.Domain.Constants;
using OnlineStore.Domain.DTO.Stock;
using OnlineStore.Domain.Enums;
using OnlineStore.Domain.Helpers;
using OnlineStore.Domain.Mapper;
using System.Net;

namespace OnlineStore.Cms.Controllers
{
    [RequiredArgsConstructor]
    [Authorize(Roles = "ADMIN,CLERK")]
    public partial class StocksController : Controller
    {
        private readonly StocksService _stocksService;
        private readonly IBaseMapper<StockDTO, StockListViewModel> _stockListViewModelMapper;
        private readonly IBaseMapper<StockDTO, BaseStockViewModel> _baseStockViewModelMapper;
        // GET: StocksController
        public ActionResult Index()
        {
            return View();
        }

        // GET: StocksController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var stock = await _stocksService.GetStockById(id);
            if (stock != null)
            {
                var new_stock = _baseStockViewModelMapper.MapModel(stock);
                return Json(new_stock);
            }
            Response.StatusCode = (int)HttpStatusCode.BadGateway;
            return Json(new { message = new { content = ErrorMessages.NOT_FOUND, type = ToastType.Info } });
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
            PaginatedDataViewModel<StockDTO> stocks = await _stocksService.GetStocksPaginate(page + 1, limit.Value, search);
            IEnumerable<StockListViewModel> stocksListViewModel = _stockListViewModelMapper.MapList(stocks.Data);
            return Json(new { rows = stocksListViewModel, total = stocks.TotalCount });
        }
        [HttpPost]
        public async Task<ActionResult> Update(StockReqViewModel stockReq)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _stocksService.UpdateStock(stockReq.StockID, stockReq.Quantity, stockReq.Reason, stockReq.type);
                    if (result != null)
                    {
                        return Json(new { stock = _baseStockViewModelMapper.MapModel(result), message = new { content = SuccessMessages.SUCCESS, type = ToastType.Success } });
                    }
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Json(new { message = new { content = ErrorMessages.NOT_FOUND, type = ToastType.Info } });
                }
                catch (Exception ex)
                {
                    //Log 
                }
            }
            Response.StatusCode = (int)HttpStatusCode.BadGateway;
            return Json(new
            {
                message = new
                {
                    content = ErrorMessages.INVALID_DATA,
                    type = ToastType.Error
                }
            });
        }

    }
}
