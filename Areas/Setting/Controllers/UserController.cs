using BackOffice.Areas.Setting.Models;
using BackOffice.Areas.Setting.Models.Service.Repository;
using BackOffice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Security.Claims;

namespace BackOffice.Areas.Setting.Controllers
{
    [Area("Setting")]
    [Authorize]
    public class UserController : Controller
    {
        private IUserRepository _user;
        public UserController(IUserRepository user)
        {
            _user= user;
        }
        private List<string> GetPermissionCodes ()
        {
            string? userIdText = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(!int.TryParse(userIdText,out int userId))
            {
                return new List<string>();
            }
            List<string> permissionCodes = _user.GetPermissionCodes(userId);
            return permissionCodes;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var permissionCodes = GetPermissionCodes();
            if (!permissionCodes.Contains("User.View"))
            {
                return StatusCode(403);
            }
            ViewBag.permissionCodes = permissionCodes;
            return View(_user.GetUserList());
        }
        [HttpGet]
        public IActionResult Create()
        {
            var permissionCodes = GetPermissionCodes();
            if (!permissionCodes.Contains("User.Create"))
            {
                return StatusCode(403);
            }
            ViewBag.permissionCodes = permissionCodes;
            var roles=_user.GetRoles();
            ViewBag.RoleList = new SelectList(roles, "role_id", "role_name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User user)
        {
            var permissionCodes = GetPermissionCodes();
            if (!permissionCodes.Contains("User.Create"))
            {
                return StatusCode(403);
            }
            ViewBag.permissionCodes = permissionCodes;
            var roles = _user.GetRoles();
            ViewBag.RoleList = new SelectList(roles,"role_id","role_name");//讓了讓post送出時，背景的角色選單維持一致


            ServiceResult result = _user.Create(user);
            TempData["Success"] = result.success;
            TempData["Message"] = result.message;
            return RedirectToAction();
        }
        [HttpGet]

        public IActionResult Edit(int id)
        {
            var permissionCodes = GetPermissionCodes();
            if (!permissionCodes.Contains("User.Edit"))
            {
                return StatusCode(403);
            }
            ViewBag.permissionCodes = permissionCodes;
            var roles = _user.GetRoles();
            ViewBag.RoleList = new SelectList(roles, "role_id", "role_name");
            return View(_user.GetById(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(User user)
        {
            var permissionCodes = GetPermissionCodes();
            if (!permissionCodes.Contains("User.Edit"))
            {
                return StatusCode(403);
            }
            ViewBag.permissionCodes = permissionCodes;
            var roles = _user.GetRoles();
            ViewBag.RoleList = new SelectList(roles, "role_id", "role_name");//讓了讓post送出時，背景的角色選單維持一致


            ServiceResult result = _user.Update(user);
            TempData["Success"] = result.success;
            TempData["Message"] = result.message;
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var permissionCodes = GetPermissionCodes();
            if (!permissionCodes.Contains("User.Delete"))
            {
                return StatusCode(403);
            }
            ViewBag.permissionCodes = permissionCodes;
            ServiceResult result = _user.Delete(id);
            TempData["Success"] = result.success;
            TempData["Message"] = result.message;
            return RedirectToAction("Index");
        }

    }
}
