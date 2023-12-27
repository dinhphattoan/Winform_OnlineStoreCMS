using Lombok.NET;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.BusinessLogic.Services;
using OnlineStore.Cms.ViewModels.Categories;
using OnlineStore.Domain.Constants;
using OnlineStore.Domain.DTO.Category;
using OnlineStore.Domain.Enums;
using OnlineStore.Domain.Helpers;
using OnlineStore.Domain.Mapper;
using System.Net;

namespace OnlineStore.Cms.Controllers
{
    [RequiredArgsConstructor]
    [Authorize(Roles = "ADMIN,CLERK")]
    public partial class CategoriesController : Controller
    {
        private readonly CategoriesService _categoryService;
        private readonly CloudinaryService _cloudinaryService;
        private readonly IBaseMapper<CategoryDTO, CategoriesListViewModel> _categoryListViewModelMapper;
        private readonly IBaseMapper<CategoriesReqViewModel, CategoryDTO> _categoryDTOMapper;
        private readonly IBaseMapper<CategoryDTO, CategoriesViewModel> _categoryViewModelMapper;
        // GET: CategoriesController
        public async Task<ActionResult> Index()
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
            PaginatedDataViewModel<CategoryDTO> categories = await _categoryService.GetCategoriesPaginate(page + 1, limit.Value, search);
            IEnumerable<CategoriesListViewModel> categoriesListViewModel = _categoryListViewModelMapper.MapList(categories.Data);
            return Json(new { rows = categoriesListViewModel, total = categories.TotalCount });
        }
        // GET: CategoriesController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var category = await _categoryService.GetByID(id);
            if (category == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { message = new { content = ErrorMessages.CATEGORY_NOT_FOUND, type = ToastType.Info } });
            }
            var result = _categoryViewModelMapper.MapModel(category);
            return Json(result);
        }

        // GET: CategoriesController/Create
        [HttpPost]
        public async Task<ActionResult> Create(CategoriesReqViewModel categoryReq)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CategoryDTO categoryDTO = _categoryDTOMapper.MapModel(categoryReq);
                    if (categoryReq.Image != null)
                    {
                        categoryDTO.Image = await _cloudinaryService.UploadPictureAsync(categoryReq.Image, categoryReq.Image.FileName);
                    }
                    var new_category = await _categoryService.Create(categoryDTO);
                    string msg = SuccessMessages.SUCCESS_CREATE_CATEGORY;
                    ToastType msg_type = ToastType.Success;
                    var result = new { category = _categoryListViewModelMapper.MapModel(new_category), message = new { content = msg, type = msg_type } };
                    return Json(result);
                }
                catch (Exception ex)
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Json(new { message = new { content = ErrorMessages.FAILED_TO_CREATE_CATEGORY, type = ToastType.Error } });
                }

            }
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return Json(new { message = new { content = ErrorMessages.INVALID_DATA, type = ToastType.Error } });
        }


        // Post: CategoriesController/Edit
        [HttpPost]
        public async Task<ActionResult> Edit(CategoriesReqViewModel categoryReq)
        {
            if (ModelState.IsValid)
            {
                CategoryDTO categoryDTO = _categoryDTOMapper.MapModel(categoryReq);
                if (categoryReq.Image != null)
                {
                    categoryDTO.Image = await _cloudinaryService.UploadPictureAsync(categoryReq.Image, categoryReq.Image.FileName);
                }
                var new_category = await _categoryService.Update(categoryDTO);
                string msg = SuccessMessages.SUCCESS_UPDATE_CATEGORY;
                ToastType msg_type = ToastType.Success;
                var result = new { category = _categoryListViewModelMapper.MapModel(new_category), message = new { content = msg, type = msg_type } };
                return Json(result);
            }
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return Json(new { message = new { content = ErrorMessages.FAILED_TO_CREATE_CATEGORY, type = ToastType.Error } });
        }

        // POST: CategoriesController/Edit/5

        // GET: CategoriesController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {

            Tuple<CategoryDTO, bool> data = await _categoryService.Delete(id);
            if (data != null)
            {
                string msg = SuccessMessages.SUCCESS_DELETE_CATEGORY;
                ToastType msg_type = ToastType.Success;
                var result = new { category = _categoryListViewModelMapper.MapModel(data.Item1), isDelete = data.Item2, message = new { content = msg, type = msg_type } };
                return Json(result);
            }
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return Json(new { message = new { content = ErrorMessages.CATEGORY_NOT_FOUND, type = ToastType.Info } });
        }

        public async Task<ActionResult> Enable(int id)
        {
            CategoryDTO category = await _categoryService.Enable(id);
            if (category != null)
            {
                string msg = SuccessMessages.SUCCESS;
                ToastType msg_type = ToastType.Success;
                var result = new { category = _categoryListViewModelMapper.MapModel(category), message = new { content = msg, type = msg_type } };
                return Json(result);
            }
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return Json(new { message = new { content = ErrorMessages.CATEGORY_NOT_FOUND, type = ToastType.Info } });
        }
    }
}
