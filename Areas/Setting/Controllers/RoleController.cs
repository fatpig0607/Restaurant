using BackOffice.Areas.Setting.Models;
using BackOffice.Areas.Setting.Models.Service.Repository;
using BackOffice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BackOffice.Areas.Setting.Controllers
{
    [Area("Setting")]
    [Authorize]
    public class RoleController : Controller
    {
        private IRoleRepository _role;
        private IUserRepository _user;
        public RoleController(IRoleRepository role,IUserRepository user)
        {
            _user = user;
            _role = role;
        }
        private List<string> GetPermissionCodes ()
        {
            string? userIdText = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(!int.TryParse(userIdText,out int userId))
            {
                return new List<string>();
            }
            List<string> permissionCodes = _user.GetPermissionCodes(userId);
            //return permissionCodes.Contains(PermissionCode);
            return permissionCodes;

        }
        [HttpGet]
        public IActionResult Index()
        {
            var permissionCodes = GetPermissionCodes();
            if (!permissionCodes.Contains("Role.View"))
            {
                return StatusCode(403);
            }

            ViewBag.permissionCodes = permissionCodes;
            return View(_role.GetAll());
        }
        [HttpGet]
         public IActionResult Create()
        {
            var permissionCodes = GetPermissionCodes();
            if (!permissionCodes.Contains("Role.Create"))
            {
                return StatusCode(403);
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Role role)
        {
            var permissionCodes = GetPermissionCodes();
            if (!permissionCodes.Contains("Role.Create"))
            {
                return StatusCode(403);
            }
            ServiceResult result = _role.Create(role);
            TempData["Success"] = result.success;
            TempData["Message"] = result.message;
            return RedirectToAction();
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var permissionCodes = GetPermissionCodes();
            if (!permissionCodes.Contains("Role.Edit"))
            {
                return StatusCode(403);
            }
            return View(_role.GetById(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Role role)
        {
            var permissionCodes = GetPermissionCodes();
            if (!permissionCodes.Contains("Role.Edit"))
            {
                return StatusCode(403);
            }
            ServiceResult result = _role.Update(role);
            TempData["Success"] = result.success;
            TempData["Message"] = result.message;
            return RedirectToAction();
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var q = _role.GetById(id);
            return View(q);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var permissionCodes = GetPermissionCodes();
            if (!permissionCodes.Contains("Role.Delete"))
            {
                return StatusCode(403);
            }
            ServiceResult result = _role.Delete(id);
            //TempData["Success"] = result.success;
            TempData["Message"] = result.message;
            return RedirectToAction("index");
        }
        [HttpGet]
        public IActionResult Permissions(int id)
        {
            var permissionCodes = GetPermissionCodes();
            if (!permissionCodes.Contains("Role.Edit"))
            {
                return StatusCode(403);
            }
            var rp = _role.GetPermissions(id);
            if (rp == null)
            {
                return NotFound();
            }
            return View(rp);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Permissions(int id,List<int> selected_permissions_ids) //id從網址來；selected_permissions_ids從表單name來
        {
            var permissionCodes = GetPermissionCodes();
            if (!permissionCodes.Contains("Role.Edit"))
            {
                return StatusCode(403);
            }
            ServiceResult result=_role.SavePermissions(id, selected_permissions_ids);

            if (result.success)
            {
                TempData["Message"] = result.message;
                return RedirectToAction("Index");
            }
            var model = _role.GetPermissions(id);
            if (model == null)
            {
                return NotFound();
            }
            model.selected_permissions_ids = selected_permissions_ids;
            TempData["Message"]=result.message;
            return View(model);
        }
    }
}
