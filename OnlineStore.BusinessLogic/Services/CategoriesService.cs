using Lombok.NET;
using OnlineStore.DataAccess;
using OnlineStore.Domain.DTO.Category;
using OnlineStore.Domain.Exceptions;
using OnlineStore.Domain.Helpers;
using OnlineStore.Domain.Mapper;
using OnlineStore.Domain.Models;
using System.Linq.Expressions;
using System.Net;
namespace OnlineStore.BusinessLogic.Services
{
	[RequiredArgsConstructor]
	public partial class CategoriesService
	{
		private readonly UnitOfWork _unitOfWork;
		private readonly IBaseMapper<Category, CategoryDTO> _categoryDTOMapper;
		private readonly IBaseMapper<CategoryDTO, Category> _categoryMapper;
		public async Task<IEnumerable<CategoryDTO>> GetAllActiveCategories()
		{
			var categories = await _unitOfWork.CategoriesRepository.GetAllActiveCategories();
			return _categoryDTOMapper.MapList(categories);
		}
		public async Task<IEnumerable<CategoryDTO>> GetAllForClient()
		{
			List<Expression<Func<Category, bool>>> filters = new List<Expression<Func<Category, bool>>>() { c => c.IsDeleted == false };
			var categories = await _unitOfWork.CategoriesRepository.Get(filters);
			if (categories == null)
			{
				throw new HttpStatusCodeException(HttpStatusCode.NotFound, "No category found!");
			}
			IEnumerable<CategoryDTO> categoriesDTOs = _categoryDTOMapper.MapList(categories);

			return categoriesDTOs;
		}
		public async Task<PaginatedDataViewModel<CategoryDTO>> GetCategoriesPaginate(int PageNumber, int PageSize, string? search)
		{
			Func<IQueryable<Category>, IOrderedQueryable<Category>> orderBy = q => q.OrderByDescending(c => c.Name);
			List<Expression<Func<Category, bool>>> filters = new List<Expression<Func<Category, bool>>>();
			if (search != null)
			{
				filters.Add(c => c.Name.ToLower().Contains(search));
			}
			PaginatedDataViewModel<Category> paginatedDataViewModel = await _unitOfWork.CategoriesRepository.GetPaginatedData(filters, orderBy, null, PageNumber, PageSize);
			IEnumerable<Category> categories = paginatedDataViewModel.Data;
			IEnumerable<CategoryDTO> categoriesDTO = _categoryDTOMapper.MapList(categories);
			PaginatedDataViewModel<CategoryDTO> new_paginatedDataViewModel = new PaginatedDataViewModel<CategoryDTO>(categoriesDTO, paginatedDataViewModel.TotalCount);
			return new_paginatedDataViewModel;
		}

		public async Task<CategoryDTO> Create(CategoryDTO categoryDTO)
		{
			try
			{
				Category category = _categoryMapper.MapModel(categoryDTO);
				await _unitOfWork.CategoriesRepository.Create(category);
				_unitOfWork.Save();
				return _categoryDTOMapper.MapModel(category);
			}
			catch (Exception e)
			{
				return null;
			}
		}

		public async Task<CategoryDTO> GetByID(int id)
		{
			try
			{
				Category category = await _unitOfWork.CategoriesRepository.GetById(id);
				return _categoryDTOMapper.MapModel(category);
			}
			catch (Exception e)
			{
				return null;
			}
		}

		public async Task<CategoryDTO> Update(CategoryDTO categoryDTO)
		{
			try
			{
				if (categoryDTO.Image == null)
				{
					Category old_category = await _unitOfWork.CategoriesRepository.GetById(categoryDTO.CategoryID);
					categoryDTO.Image = old_category.Image;
				}
				Category category = _categoryMapper.MapModel(categoryDTO);
				await _unitOfWork.CategoriesRepository.Update(category);
				_unitOfWork.Save();
				return _categoryDTOMapper.MapModel(category);
			}
			catch (Exception e)
			{
				return null;
			}
		}
		public async Task<Tuple<CategoryDTO, bool>> Delete(int id)
		{
			try
			{
				bool isCategoryUsedInProducts = await _unitOfWork.ProductRepository.CategoryHasProducts(id);
				Category category = await _unitOfWork.CategoriesRepository.GetById(id);
				if (isCategoryUsedInProducts)
				{
					category.IsDeleted = true;
				}
				else
				{
					await _unitOfWork.CategoriesRepository.Delete(category);
				}
				_unitOfWork.Save();
				return Tuple.Create(_categoryDTOMapper.MapModel(category), !isCategoryUsedInProducts);
			}
			catch (Exception e)
			{
				return null;
			}
		}

		public async Task<CategoryDTO> Enable(int id)
		{
			try
			{
				Category category = await _unitOfWork.CategoriesRepository.GetById(id);
				category.IsDeleted = false;
				await _unitOfWork.CategoriesRepository.Update(category);
				_unitOfWork.Save();
				return _categoryDTOMapper.MapModel(category);
			}
			catch (Exception e)
			{
				return null;
			}
		}
	}
}
