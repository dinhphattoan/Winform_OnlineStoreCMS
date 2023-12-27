using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Cms.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.IsInRole(Role.ADMIN.ToString()))
            {
                return RedirectToAction("Index", "Users");
            }

            if (User.IsInRole(Role.CLERK.ToString()))
            {
                return RedirectToAction("Index", "Products");
            }

            if (User.Identity.IsAuthenticated)
            {
                // TODO: Create a view for this
                return View("AccessDenied");
            }
            else
            {
                return RedirectToAction("Login", "Authentication");
            }
        }
    }
}
