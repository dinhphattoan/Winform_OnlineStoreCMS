using OnlineStore.Domain.DTO.OrderDetail;
using OnlineStore.Domain.DTO.User;

namespace OnlineStore.Domain.DTO.Order
{
	public class OrderDTO : BaseOrderDTO
	{
		public UserDTO? Clerk { get; set; }
		public UserDTO? Customer { get; set; }
		public DateTimeOffset? CreatedAt { get; set; }
		public bool IsDeleted { get; set; }
		public List<OrderDetailDTO>? OrderDetails { get; set; }
	}
}
