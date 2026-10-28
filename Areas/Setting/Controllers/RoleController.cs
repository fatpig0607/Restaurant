using BackOffice.Areas.Setting.Models;
using BackOffice.Areas.Setting.Models.Service.Repository;
using BackOffice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackOffice.Areas.Setting.Controllers
{
    [Area("Setting")]
    [Authorize]
    public class RoleController : Controller
    {
        private IRoleRepository _role;
        public RoleController(IRoleRepository role)
        {
            _role = role;
        }
        public IActionResult Index()
        {
            return View(_role.GetAll());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Role role)
        {
            ServiceResult result = _role.Create(role);
            TempData["Success"] = result.success;
            TempData["Message"] = result.message;
            return RedirectToAction();
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View(_role.GetById(id));
        }
        [HttpPost]
        public IActionResult Edit(Role role)
        {
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
        public IActionResult Delete(int id)
        {
            ServiceResult result = _role.Delete(id);
            //TempData["Success"] = result.success;
            TempData["Message"] = result.message;
            return RedirectToAction("index");
        }
        [HttpGet]
        public IActionResult Permissions(int id)
        {
            var rp = _role.GetPermissions(id);
            if (rp == null)
            {
                return NotFound();
            }
            return View(rp);

        }
    }
}
