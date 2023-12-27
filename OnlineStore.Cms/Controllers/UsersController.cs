using Lombok.NET;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.BusinessLogic.Services;
using OnlineStore.Cms.ViewModels.Users;
using OnlineStore.Domain.Constants;
using OnlineStore.Domain.DTO.User;
using OnlineStore.Domain.Enums;
using OnlineStore.Domain.Helpers;
using OnlineStore.Domain.Mapper;
using System.Net;

namespace OnlineStore.Cms.Controllers
{
    [RequiredArgsConstructor]
    [Authorize(Roles = "ADMIN")]
    public partial class UsersController : Controller
    {
        private readonly UserService _userService;
        private readonly IBaseMapper<UserDTO, UserListViewModel> _userListViewModelMapper;
        private readonly IBaseMapper<UserReqViewModel, UserDTO> _userDTOModelMapper;
        private readonly IBaseMapper<UserDTO, UserViewModel> _userViewModelMapper;
        private readonly IBaseMapper<UserUpdateViewModel, UserDTO> _userDTOMapper;
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                try
                {
                    var user = await _userService.GetById(id.Value);
                    return Json(_userViewModelMapper.MapModel(user));
                }
                catch
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Json(new { message = new { content = ErrorMessages.NOT_FOUND, type = ToastType.Error } });
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadGateway;
                return Json(new { message = new { content = ErrorMessages.INVALID_DATA, type = ToastType.Error } });
            }
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
            PaginatedDataViewModel<UserDTO> userDTOs = await _userService.GetPaginated(page + 1, limit.Value, search);
            IEnumerable<UserListViewModel> userListView = _userListViewModelMapper.MapList(userDTOs.Data);
            return Json(new { rows = userListView, total = userDTOs.TotalCount });
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserReqViewModel userReq)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UserDTO userDTO = _userDTOModelMapper.MapModel(userReq);
                    UserDTO newUserDTO = await _userService.Create(userDTO);
                    string msg = SuccessMessages.SUCCESS_CREATE_USER;
                    ToastType msg_type = ToastType.Success;
                    return Json(new { user = _userListViewModelMapper.MapModel(newUserDTO), message = new { content = msg, type = ToastType.Success } });
                }
                catch (Exception ex)
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Json(new { message = new { content = ErrorMessages.FAILED_TO_CREATE_USER, type = ToastType.Error } });
                }

            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { message = new { content = ErrorMessages.FAILED_TO_CREATE_CATEGORY, type = ToastType.Error } });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateViewModel userReq)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UserDTO userDTO = _userDTOMapper.MapModel(userReq);

                    var new_user = await _userService.Update(userDTO);
                    return Json(new { user = _userListViewModelMapper.MapModel(new_user), message = new { content = SuccessMessages.SUCCESS_UPDATE_USER, type = ToastType.Success } });
                }
                catch
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Json(new { message = new { content = ErrorMessages.FAILED_TO_UPDATE_USER, type = ToastType.Error } });
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadGateway;
                return Json(new { message = new { content = ErrorMessages.INVALID_DATA, type = ToastType.Error } });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Disable(int id)
        {
            if (id != null)
            {
                try
                {
                    var user = await _userService.Disable(id);
                    return Json(new { user = _userListViewModelMapper.MapModel(user), message = new { content = SuccessMessages.SUCCESS_DELETE_USER, type = ToastType.Success } });
                }
                catch
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Json(new { message = new { content = ErrorMessages.FAILED_TO_ENABLE_USER, type = ToastType.Error } });
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadGateway;
                return Json(new { message = new { content = ErrorMessages.INVALID_DATA, type = ToastType.Error } });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Enable(int id)
        {
            if (id != null)
            {
                try
                {
                    var user = await _userService.Enable(id);
                    return Json(new { user = _userListViewModelMapper.MapModel(user), message = new { content = SuccessMessages.SUCCESS_DELETE_USER, type = ToastType.Success } });
                }
                catch
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Json(new { message = new { content = ErrorMessages.FAILED_TO_ENABLE_USER, type = ToastType.Error } });
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadGateway;
                return Json(new { message = new { content = ErrorMessages.INVALID_DATA, type = ToastType.Error } });
            }
        }
    }
}