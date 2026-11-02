using BackOffice.Areas.Catalog.Models;
using BackOffice.Areas.Catalog.Models.Service.Repository;
using BackOffice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackOffice.Areas.Catalog.Controllers
{
    [Authorize]
    [Area("Catalog")]
    public class MenuCategoriesController : Controller
    {
        private IMenuCategoriesRepository _menucategories;
        public MenuCategoriesController (IMenuCategoriesRepository menucategories)
        {
            _menucategories = menucategories;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View(_menucategories.GetAll());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MenuCategories menuCategories)
        {
            ServiceResult result = _menucategories.Create(menuCategories);
            TempData["Success"] = result.success;
            TempData["Message"] = result.message;
            return RedirectToAction("Create");
        }
    }
}
