using Lombok.NET;
using OnlineStore.DataAccess;
using OnlineStore.Domain.DTO.Stock;
using OnlineStore.Domain.DTO.StockEvent;
using OnlineStore.Domain.Enums;
using OnlineStore.Domain.Helpers;
using OnlineStore.Domain.Mapper;
using OnlineStore.Domain.Models;
using System.Linq.Expressions;

namespace OnlineStore.BusinessLogic.Services
{
	[RequiredArgsConstructor]
	public partial class StocksService
	{
		private readonly UnitOfWork _unitOfWork;
		private readonly IBaseMapper<StockDTO, Stock> _stockMapper;
		private readonly IBaseMapper<Stock, StockDTO> _stockDTOMapper;
		private readonly IBaseMapper<StockEventDTO, StockEvent> _stockEventMapper;

		public async Task<PaginatedDataViewModel<StockDTO>> GetStocksPaginate(int PageNumber, int PageSize, string? search)
		{
			Expression<Func<Stock, object>>[] include =
			{
				s => s.Product,
				s => s.Product.Category
			};
			List<Expression<Func<Stock, bool>>> filters = new List<Expression<Func<Stock, bool>>>();
			if (search != null)
			{
				filters.Add(s => s.Product.Name.ToLower().Contains(search) || s.Product.Category.Name.ToLower().Contains(search));
			}
			PaginatedDataViewModel<Stock> paginatedDataViewModel = await _unitOfWork.StockRepository.GetPaginatedData(filters, null, include, PageNumber, PageSize);

			IEnumerable<Stock> stocks = paginatedDataViewModel.Data;
			IEnumerable<StockDTO> stockDTOs = _stockDTOMapper.MapList(stocks);
			PaginatedDataViewModel<StockDTO> new_paginatedDataViewModel = new PaginatedDataViewModel<StockDTO>(stockDTOs, paginatedDataViewModel.TotalCount);
			return new_paginatedDataViewModel;
		}

		public async Task<StockDTO> GetStockById(int id)
		{
			return _stockDTOMapper.MapModel(await _unitOfWork.StockRepository.GetById(id));
		}

		public async Task<StockDTO> UpdateStock(int id, int quantity, string? reason, StockEventTypes type)
		{
			try
			{
				Stock stock = await _unitOfWork.StockRepository.GetById(id);
				if (type.Equals(StockEventTypes.IN))
				{
					stock.Quantity += quantity;
				}
				else
				{
					if (stock.Quantity < quantity)
					{
						throw new InvalidDataException("Quantity is not enough");
					}
					stock.Quantity -= quantity;
				}
				StockEvent stockEvent = new StockEvent()
				{
					Stock = stock,
					Reason = reason,
					Type = type,
					Quantity = quantity,
					CreatedAt = DateTimeOffset.Now
				};
				await _unitOfWork.StockEventRepository.Create(stockEvent);
				await _unitOfWork.StockRepository.Update(stock);
				_unitOfWork.Save();

				return _stockDTOMapper.MapModel(stock);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return null;
			}
		}
	}
}
