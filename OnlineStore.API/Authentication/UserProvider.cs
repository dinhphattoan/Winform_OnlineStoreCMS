using OnlineStore.Domain.Exceptions;
using System.Net;
using System.Security.Claims;

namespace OnlineStore.Api.Authentication
{
	public class UserProvider : IUserProvider
	{
		private readonly IHttpContextAccessor _context;
		public UserProvider(IHttpContextAccessor context)
		{
			this._context = context ?? throw new ArgumentNullException(nameof(context));
		}
		public int GetUserId()
		{
			try
			{
				string userID = _context.HttpContext.User.Claims
					.First(i => i.Type == ClaimTypes.NameIdentifier).Value;
				if (userID == null) throw new HttpStatusCodeException(HttpStatusCode.Unauthorized, "Unauthorized");
				return int.Parse(userID);
			}
			catch (Exception)
			{
				throw new HttpStatusCodeException(HttpStatusCode.Unauthorized, "Unauthorized");
			}

		}
	}
}
