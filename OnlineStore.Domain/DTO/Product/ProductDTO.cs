using OnlineStore.Domain.DTO.Category;
using OnlineStore.Domain.DTO.ProductImage;
using OnlineStore.Domain.DTO.User;

namespace OnlineStore.Domain.DTO.Product
{
    public class ProductDTO : BaseProductDTO
    {
        public BaseCategoyDTO Category { get; set; }

        public List<BaseProductImageDTO> ProductImages { get; set; }

        public bool IsDeleted { get; set; }

        public BaseUserDTO CreateBy { get; set; }
    }
}
