using Lombok.NET;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Api.ViewModel.Categories;
using OnlineStore.BusinessLogic.Services;
using OnlineStore.Domain.DTO.Category;
using OnlineStore.Domain.Mapper;

namespace OnlineStore.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[RequiredArgsConstructor]
	public partial class CategoriesController : ControllerBase
	{
		private readonly CategoriesService _categoriesService;
		private readonly IBaseMapper<CategoryDTO, BaseCategory> _categoryMapper;
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			IEnumerable<CategoryDTO> categories = await _categoriesService.GetAllForClient();
			IEnumerable<BaseCategory> categoriesRes = _categoryMapper.MapList(categories);
			return Ok(categoriesRes);
		}

		// GET api/<CategoriesController>/5
		[HttpGet("{id}")]
		public string Get(int id)
		{
			return "value";
		}
	}
}
