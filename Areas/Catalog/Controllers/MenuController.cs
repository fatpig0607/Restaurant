using BackOffice.Areas.Catalog.Models;
using BackOffice.Areas.Catalog.Models.Service.Repository;
using BackOffice.Areas.Setting.Models.Service.Repository;
using BackOffice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace BackOffice.Areas.Catalog.Controllers
{
    [Authorize]
    [Area("Catalog")]
    public class MenuController : Controller
    {
        private IMenuRepository _menu;
        private IUserRepository _user;
        private IMenuCategoriesRepository _menuCategories;
        public MenuController(IMenuRepository menu, IMenuCategoriesRepository menuCategories,IUserRepository user)
        {
            _menu = menu;
            _menuCategories = menuCategories;
            _user = user;
        }
        private List<string> GetPermissionCodes()
        {
            string? userTextId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(int.TryParse(userTextId,out int userId))
            {
                return _user.GetPermissionCodes(userId);
            }
            else
            {
                return new List<string>();
            }
        }
        [HttpGet]
        public IActionResult Index(int id)
        {
            var PermissionCode = GetPermissionCodes();
            if (PermissionCode.Contains("Menu.View"))
            {
                ViewBag.CategoryList = _menuCategories.GetAll();
                ViewBag.SelectedCategoryId = id;
                ViewBag.permissionCodes = PermissionCode;
                var q = _menu.GetMenuList(id);
                return View(q);
            }
            else
            {
                return StatusCode(403);
            }
        }
        [HttpGet]
        public IActionResult Create()
        {
            if (GetPermissionCodes().Contains("Menu.Create"))
            {
                var CategoryList = _menu.GetMenuCategories();
                ViewBag.CategoryList = new SelectList(CategoryList, "category_id", "category_name");
                return View();

            }
            else
            {
                return StatusCode(403);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Menu menu,IFormFile? menuImage)
        {
            if (GetPermissionCodes().Contains("Menu.Create"))
            {
                ServiceResult result = _menu.Create(menu, menuImage);
                TempData["Success"]=result.success;
                TempData["Message"] = result.message;
                return RedirectToAction("Create");
            }
            else
            {
                return StatusCode(403);
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (GetPermissionCodes().Contains("Menu.Edit"))
            {
                var CategoryList = _menu.GetMenuCategories();
                ViewBag.CategoryList = new SelectList(CategoryList, "category_id", "category_name");
                var q= _menu.GetById(id);
                return View(q);
            }
            else
            {
                return StatusCode(403);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Menu menu,IFormFile? menuImage, bool removeImage)
        {
            if (GetPermissionCodes().Contains("Menu.Edit"))
            {
                ServiceResult result = _menu.Edit(menu, menuImage,removeImage);
                TempData["Success"]=result.success;
                TempData["Message"]=result.message;
                return RedirectToAction("Edit");
            }
            else
            {
                return StatusCode(403);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            if (GetPermissionCodes().Contains("Menu.Delete"))
            {
                ServiceResult result =  _menu.Delete(id);
                TempData["Success"] = result.success;
                TempData["Message"] = result.message;
                return RedirectToAction("Index");
            }
            else
            {
                return StatusCode(403);
            }
        }
    }
}
