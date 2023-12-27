using Lombok.NET;
using Microsoft.AspNetCore.Http;
using OnlineStore.DataAccess;
using OnlineStore.Domain.Constants;
using OnlineStore.Domain.DTO.Product;
using OnlineStore.Domain.DTO.ProductImage;
using OnlineStore.Domain.Enums;
using OnlineStore.Domain.Exceptions;
using OnlineStore.Domain.Helpers;
using OnlineStore.Domain.Mapper;
using OnlineStore.Domain.Models;
using System.Linq.Expressions;
using System.Net;
namespace OnlineStore.BusinessLogic.Services
{
	[RequiredArgsConstructor]
	public partial class ProductsService
	{
		private readonly IBaseMapper<Product, ProductDTO> _productDTOMapper;
		private readonly IBaseMapper<ProductDTO, Product> _productMapper;

		//private readonly ProductsRepository _productRepository;
		private readonly UnitOfWork _unitOfWork;
		private readonly CloudinaryService cloudinaryService;

		public async Task<IEnumerable<ProductDTO>> GetAllProduct()
		{
			return _productDTOMapper.MapList(await _unitOfWork.ProductRepository.GetAll());
		}

		public async Task<ProductDTO> GetProductById(int id)
		{
			return _productDTOMapper.MapModel(await _unitOfWork.ProductRepository.GetById(id));
		}
		public async Task<ProductDTO> GetProductByIdForClient(int id)
		{
			try
			{
				var product = await _unitOfWork.ProductRepository.GetByIdForClient(id);
				return _productDTOMapper.MapModel(product);
			}
			catch (NotFoundException e)
			{
				throw new HttpStatusCodeException(HttpStatusCode.NotFound, e);
			}
		}
		public async Task<ProductDTO> CreateProduct(ProductDTO productDTO, List<IFormFile>? images, int? quantity)
		{
			if (images != null)
			{
				List<String> images_urls = new List<string>();
				foreach (var image in images)
				{
					if (image.Length > 0)
					{
						images_urls.Add(await cloudinaryService.UploadPictureAsync(image, image.FileName));
					}
				}
				int index = 0;
				List<BaseProductImageDTO> baseProductImageDTOs = new List<BaseProductImageDTO>();
				foreach (var image_url in images_urls)
				{
					baseProductImageDTOs.Add(new BaseProductImageDTO
					{
						Order = index,
						Path = image_url
					});
					index++;
				}
				productDTO.ProductImages = baseProductImageDTOs;
				productDTO.Thumbnail = images_urls[Int32.Parse(productDTO.Thumbnail)];
			}
			if (quantity == null)
			{
				quantity = 0;
			}

			Product product = _productMapper.MapModel(productDTO);
			Stock stock = new Stock
			{
				Quantity = quantity.Value

			};
			StockEvent stockEvent = new StockEvent
			{
				Quantity = quantity.Value,
				Type = StockEventTypes.IN,
				Stock = stock,
				Reason = "Create product",
				CreatedAt = DateTime.Now
			};
			// TODO: get user id from token
			product.CreateByID = 1;//for test
			product.CreatAt = DateTime.Now;
			product.Stock = stock;
			foreach (var ProductImage in product.ProductImages)
			{
				ProductImage.Product = product;
			}
			var createdProduct = await _unitOfWork.ProductRepository.Create(product);
			await _unitOfWork.StockEventRepository.Create(stockEvent);

			_unitOfWork.Save();
			return _productDTOMapper.MapModel(createdProduct);
		}

		public async Task<PaginatedDataViewModel<ProductDTO>> GetProductsPaginate(int PageNumber, int PageSize, string? search)
		{
			Expression<Func<Product, object>>[] include =
			{
				p => p.ProductImages,
				p => p.Category,
				p => p.Stock
			};
			List<Expression<Func<Product, bool>>> filters = new List<Expression<Func<Product, bool>>>();
			if (search != null)
			{
				filters.Add(p => p.Name.ToLower().Contains(search) || p.Category.Name.ToLower().Contains(search));
			}
			PaginatedDataViewModel<Product> paginatedDataViewModel = await _unitOfWork.ProductRepository.GetPaginatedData(filters, null, include, PageNumber, PageSize);
			IEnumerable<Product> products = paginatedDataViewModel.Data;
			IEnumerable<ProductDTO> productCateDTOs = _productDTOMapper.MapList(products);
			PaginatedDataViewModel<ProductDTO> new_paginatedDataViewModel = new PaginatedDataViewModel<ProductDTO>(productCateDTOs, paginatedDataViewModel.TotalCount);
			return new_paginatedDataViewModel;
		}
		public async Task<PaginatedDataViewModel<ProductDTO>> GetProductsPaginateForClient(int PageNumber, int PageSize, string? search, string sortBy, string? order, int? categoryID)
		{
			Expression<Func<Product, object>>[] include =
			{
				p => p.ProductImages,
				p => p.Category,
				p => p.Stock
			};
			List<Expression<Func<Product, bool>>> filters = new List<Expression<Func<Product, bool>>>();
			filters.Add(p => !p.IsDeleted);
			if (search != null)
			{
				filters.Add(p => p.Name.ToLower().Contains(search) || p.Category.Name.ToLower().Contains(search));
			}
			if (categoryID != null && !categoryID.Equals(-1))
			{
				filters.Add(p => p.Category.CategoryID.Equals(categoryID));
			}
			Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy = null;
			if (sortBy != null && order != null)
			{
				if (sortBy.Equals("name"))
				{
					if (order.Equals("asc"))
					{
						orderBy = q => q.OrderBy(p => p.Name);
					}
					else
					{
						orderBy = q => q.OrderByDescending(p => p.Name);
					}
				}
				else if (sortBy.Equals("price"))
				{
					if (order.Equals("asc"))
					{
						orderBy = q => q.OrderBy(p => p.UnitPrice);
					}
					else
					{
						orderBy = q => q.OrderByDescending(p => p.UnitPrice);
					}
				}
			}

			PaginatedDataViewModel<Product> paginatedDataViewModel = await _unitOfWork.ProductRepository.GetPaginatedData(filters, orderBy, include, PageNumber, PageSize);
			IEnumerable<Product> products = paginatedDataViewModel.Data;
			IEnumerable<ProductDTO> productCateDTOs = _productDTOMapper.MapList(products);
			PaginatedDataViewModel<ProductDTO> new_paginatedDataViewModel = new PaginatedDataViewModel<ProductDTO>(productCateDTOs, paginatedDataViewModel.TotalCount);
			return new_paginatedDataViewModel;
		}

		public async Task<string> UpdateProduct(ProductDTO productDTO, List<int>? imageDeletes, List<IFormFile>? new_images)
		{
			Product product_org = await _unitOfWork.ProductRepository.GetById(productDTO.ProductID);
			if (product_org == null)
			{
				return ErrorMessages.PRODUCT_NOT_FOUND;
			}
			// no change quantity and createby
			product_org.Name = productDTO.Name;
			product_org.Description = productDTO.Description;
			product_org.UnitPrice = productDTO.UnitPrice;
			product_org.CategoryID = productDTO.Category.CategoryID;

			//delete image
			if (imageDeletes != null && product_org.ProductImages != null)
			{
				// image
				// imageDetelte is productImageID O(n2)
				foreach (var imageDelete in imageDeletes)
				{
					foreach (var productImage in product_org.ProductImages)
					{
						if (productImage.ProductImageID == imageDelete)
						{
							product_org.ProductImages.Remove(productImage);
							await _unitOfWork.ProductImageRepository.Delete(productImage);
							break;
						}
					}
				}

				// đánh lại index
				for (int i = 0; i < product_org.ProductImages.Count; i++)
				{
					product_org.ProductImages[i].Order = i;
				}

			}

			// add new images
			if (new_images != null)
			{
				List<string> images_urls = new List<string>();
				foreach (var image in new_images)
				{
					if (image.Length > 0)
					{
						images_urls.Add(await cloudinaryService.UploadPictureAsync(image, image.FileName));
					}
				}
				List<BaseProductImageDTO> baseProductImageDTOs = new List<BaseProductImageDTO>();
				// co 5 anh, xoa 2 anh, them 3 anh, thi se co 6 anh, index = 3
				int imageDeleteCount = imageDeletes?.Count ?? 0; //
				int product_org_imageCount = product_org.ProductImages?.Count ?? 0;
				int index = product_org_imageCount - imageDeleteCount;
				index = Math.Max(0, index);
				foreach (var image_url in images_urls)
				{
					baseProductImageDTOs.Add(new BaseProductImageDTO
					{
						Order = index,
						Path = image_url
					});
					index++;
				}

				// check if product has no image
				if (product_org.ProductImages == null)
				{
					product_org.ProductImages = new List<ProductImage>();
				}
				// add new images
				foreach (var ProductImage in baseProductImageDTOs)
				{
					product_org.ProductImages.Add(new ProductImage
					{
						Order = ProductImage.Order,
						Path = ProductImage.Path,
						Product = product_org
					});
				}
			}
			//set thumbnail
			if (productDTO.Thumbnail != null)
			{
				int product_org_imageCount = product_org.ProductImages?.Count ?? 0;
				int thumnailIndex = Int32.Parse(productDTO.Thumbnail);
				if (thumnailIndex < product_org_imageCount) // nho hon chu k nho hon bang vi index tinh tu 0
				{
					product_org.Thumbnail = product_org.ProductImages[Int32.Parse(productDTO.Thumbnail)].Path;
				}
			}

			await _unitOfWork.ProductRepository.Update(product_org);
			_unitOfWork.Save();
			return SuccessMessages.UPDATE_SUCCESS;
		}

		public async Task<string> ToggleProduct(int id, bool status)
		{
			Product product = await _unitOfWork.ProductRepository.GetById(id);
			if (product == null)
			{
				return ErrorMessages.PRODUCT_NOT_FOUND;
			}
			product.IsDeleted = status;
			await _unitOfWork.ProductRepository.Update(product);
			_unitOfWork.Save();
			return SuccessMessages.DELETE_SUCCESS;
		}

		public async Task<LinkedList<ProductDTO>> getByIds(LinkedList<int> ids)
		{
			LinkedList<ProductDTO> productDTOs = new LinkedList<ProductDTO>();
			foreach (var id in ids)
			{
				productDTOs.AddLast(await GetProductByIdForClient(id));
			}
			return productDTOs;
		}
	}
}
