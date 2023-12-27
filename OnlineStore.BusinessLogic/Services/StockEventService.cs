using Lombok.NET;
using OnlineStore.DataAccess;
using OnlineStore.Domain.DTO.StockEvent;
using OnlineStore.Domain.Helpers;
using OnlineStore.Domain.Mapper;
using OnlineStore.Domain.Models;
using System.Linq.Expressions;

namespace OnlineStore.BusinessLogic.Services
{
	[RequiredArgsConstructor]
	public partial class StockEventService
	{
		private readonly UnitOfWork _unitOfWork;
		private readonly IBaseMapper<StockEvent, StockEventDTO> _stockEventDTOMapper;

		public async Task<PaginatedDataViewModel<StockEventDTO>> GetStocksPaginate(int PageNumber, int PageSize, string? search)
		{
			Expression<Func<StockEvent, object>>[] include =
			{
				s => s.Stock.Product
			};

			List<Expression<Func<StockEvent, bool>>> filters = new List<Expression<Func<StockEvent, bool>>>();
			if (search != null)
			{
				filters.Add(s => s.Stock.Product.Name.ToLower().Contains(search) || s.Stock.Product.Category.Name.ToLower().Contains(search));
			}
			PaginatedDataViewModel<StockEvent> paginatedDataViewModel = await _unitOfWork.StockEventRepository.GetPaginatedData(filters, null, include, PageNumber, PageSize);
			IEnumerable<StockEvent> stocks = paginatedDataViewModel.Data;
			IEnumerable<StockEventDTO> stockEventDTOs = _stockEventDTOMapper.MapList(stocks);
			PaginatedDataViewModel<StockEventDTO> new_paginatedDataViewModel = new PaginatedDataViewModel<StockEventDTO>(stockEventDTOs, paginatedDataViewModel.TotalCount);
			return new_paginatedDataViewModel;
		}

		public async Task<StockEventDTO> GetStockById(int id)
		{
			return _stockEventDTOMapper.MapModel(await _unitOfWork.StockEventRepository.GetById(id));
		}
	}
}
