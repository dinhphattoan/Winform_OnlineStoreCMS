using Lombok.NET;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineStore.BusinessLogic.Services;
using OnlineStore.Cms.Models;
using OnlineStore.Cms.ViewModels.Product;
using OnlineStore.Domain.Constants;
using OnlineStore.Domain.DTO.Product;
using OnlineStore.Domain.Enums;
using OnlineStore.Domain.Helpers;
using OnlineStore.Domain.Mapper;
using System.Diagnostics;

namespace OnlineStore.Cms.Controllers
{
    [RequiredArgsConstructor]
    [Authorize(Roles = "ADMIN,CLERK")]
    public partial class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly ProductsService _productService;
        private readonly IBaseMapper<ProductDTO, ProductViewModel> _productViewMapper;
        private readonly CategoriesService _categoryService;
        private readonly IBaseMapper<ProductReqViewModel, ProductDTO> _productDTOMapper;
        private readonly IBaseMapper<ProductDTO, ProductReqViewModel> _productReqViewMapper;

        [HttpGet]
        public async Task<IActionResult> Index(int? p, string? search, string? m, ToastType? type)
        {
            if (p == null)
            {
                p = 1;
            }
            if (m != null && type != null)
            {
                ViewBag.Message = m;
                ViewBag.MessageType = type;
            }
            PaginatedDataViewModel<ProductDTO> products = await _productService.GetProductsPaginate(p.Value, 20, search);
            var new_products = products.Data;
            ViewBag.CountPage = (int)Math.Ceiling((double)products.TotalCount / 20);
            ViewBag.PageNumber = p;
            ViewBag.products = _productViewMapper.MapList(new_products);
            ViewBag.categories = (await _categoryService.GetAllActiveCategories())
            .Select(c => new SelectListItem { Value = c.CategoryID.ToString(), Text = c.Name })
            .ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductReqViewModel productReq)
        {
            if (ModelState.IsValid)
            {
                ProductDTO productDTO = _productDTOMapper.MapModel(productReq);
                var new_product = await _productService.CreateProduct(productDTO, productReq.Images, productReq.Quantity);
                string msg = SuccessMessages.SUCCESS_CREATE_PRODUCT;
                ToastType msg_type = ToastType.Success;
                var result = new { product = new_product, message = new { content = msg, type = msg_type } };
                //return Json(result);
                return Redirect("/?m=" + msg + "&type=" + msg_type);
            }
            string message = "Model is not valid";
            ToastType type = ToastType.Error;

            return Redirect($"/?m={message}&type={type}");
        }

        [HttpGet]
        public async Task<JsonResult> GetProductById(int? id)
        {
            ProductDTO product = await _productService.GetProductById(id.Value);
            ProductViewModel productViewModel = _productViewMapper.MapModel(product);
            ViewBag.categories = (await _categoryService.GetAllActiveCategories())
            .Select(c => new SelectListItem { Value = c.CategoryID.ToString(), Text = c.Name })
            .ToList();
            return Json(productViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductReqViewModel productReq)
        {
            // ex list 1,3,5
            List<int> imageDeletes = new List<int>();
            if (productReq.deleteImages != null)
            {

                if (productReq.deleteImages != null)
                {
                    imageDeletes = productReq.deleteImages.Split(',').Select(int.Parse).ToList();
                }
            }

            if (ModelState.IsValid)
            {
                ProductDTO productDTO = _productDTOMapper.MapModel(productReq);
                await _productService.UpdateProduct(productDTO, imageDeletes, productReq.Images);
                return Redirect("/");
            }
            string message = "Model is not valid";
            ToastType type = ToastType.Error;
            return Redirect($"/?m={message}&type={type}");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.ToggleProduct(id, true);
            string message = "Delete success";
            ToastType type = ToastType.Success;
            return Json(new { productID = id, message = message, type = type });
            //return Redirect($"/?m={message}&type={type}");
        }

        [HttpGet]
        public async Task<IActionResult> Enable(int id)
        {
            await _productService.ToggleProduct(id, false);
            string message = "Enable success";
            ToastType type = ToastType.Success;
            return Json(new { productID = id, message = message, type = type });
            //return Redirect($"/?m={message}&type={type}");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
