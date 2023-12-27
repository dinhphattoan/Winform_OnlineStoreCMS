using Lombok.NET;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Api.ViewModel.Product;
using OnlineStore.API.ViewModel.Product;
using OnlineStore.BusinessLogic.Services;
using OnlineStore.Domain.DTO.Product;
using OnlineStore.Domain.Helpers;
using OnlineStore.Domain.Mapper;


namespace OnlineStore.API.Controllers
{
	[RequiredArgsConstructor]
	[Route("api/[controller]")]
	[ApiController]
	public partial class ProductsController : ControllerBase
	{
		private readonly ProductsService _productService;
		private readonly IBaseMapper<ProductDTO, ProductRes> _productResMapper;
		private readonly IBaseMapper<ProductDTO, ProductDetailRes> _productDetailResMapper;
		// GET: api/<ProductController>
		[HttpGet]
		public async Task<IActionResult> Get(int? page, int? limit, string? search, string? sortBy, string? order, int? categoryID)
		{
			if (limit == null)
			{
				limit = 20;
			}
			PaginatedDataViewModel<ProductDTO> products = await _productService.GetProductsPaginateForClient((page != null) ? page.Value : 1, limit.Value, search, sortBy, order, categoryID);
			IEnumerable<ProductRes> productViewModel = _productResMapper.MapList(products.Data);
			return Ok(new { list = productViewModel, total = products.TotalCount });
		}

		// GET api/<ProductController>/5
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			return Ok(_productDetailResMapper.MapModel(await _productService.GetProductByIdForClient(id)));
		}

		[HttpPost("GetByIds")]
		public async Task<IActionResult> GetIds(LinkedList<int> ids)
		{
			LinkedList<ProductDTO> productDTOs = await _productService.getByIds(ids);
			LinkedList<ProductRes> productRes = new LinkedList<ProductRes>();
			foreach (var productDTO in productDTOs)
			{
				productRes.AddLast(_productResMapper.MapModel(productDTO));
			}
			return Ok(productRes);
		}
	}
}
