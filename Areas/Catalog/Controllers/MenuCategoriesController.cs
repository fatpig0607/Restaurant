using BackOffice.Areas.Catalog.Models;
using BackOffice.Areas.Catalog.Models.Service.Repository;
using BackOffice.Areas.Setting.Models.Service.Repository;
using BackOffice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Security.Claims;

namespace BackOffice.Areas.Catalog.Controllers
{
    [Authorize]
    [Area("Catalog")]
    public class MenuCategoriesController : Controller
    {
        private IMenuCategoriesRepository _menucategories;
        private IUserRepository _user;
        public MenuCategoriesController (IMenuCategoriesRepository menucategories, IUserRepository user)
        {
            _menucategories = menucategories;
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
        public IActionResult Index()
        {
            var PermissionCode = GetPermissionCodes();
            if (PermissionCode.Contains("MenuCategories.View"))
            {
                ViewBag.permissionCodes = PermissionCode;
                return View(_menucategories.GetAll());
            }
            else
            {
                return StatusCode(403);
            }
        }
        [HttpGet]
        public IActionResult Create()
        {
            if (GetPermissionCodes().Contains("MenuCategories.Create"))
            {
                return View();
            }
            else
            {
                return StatusCode(403);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MenuCategories menuCategories)
        {
            if (GetPermissionCodes().Contains("MenuCategories.Create"))
            {
                ServiceResult result = _menucategories.Create(menuCategories);
                TempData["Success"] = result.success;
                TempData["Message"] = result.message;
                return RedirectToAction("Create");
            }
            else
            {
                return StatusCode(403);
            }
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (GetPermissionCodes().Contains("MenuCategories.Edit"))
            {
                var q = _menucategories.GetById(id);
                if (q == null)
                {
                    return NotFound();
                }
                return View(q);
            }
            else
            {
                return StatusCode(403);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MenuCategories menuCategories)
        {
            if (GetPermissionCodes().Contains("MenuCategories.Edit"))
            {
                ServiceResult result = _menucategories.Edit(menuCategories);
                TempData["Success"] = result.success;
                TempData["Message"] = result.message;
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
            if (GetPermissionCodes().Contains("MenuCategories.Delete"))
            {
                ServiceResult result = _menucategories.Delete(id);
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
