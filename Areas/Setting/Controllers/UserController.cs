using BackOffice.Areas.Setting.Models;
using BackOffice.Areas.Setting.Models.Service.Repository;
using BackOffice.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BackOffice.Areas.Setting.Controllers
{
    [Area("Setting")]
    public class UserController : Controller
    {
        private IUserRepository _user;
        public UserController(IUserRepository user)
        {
            _user= user;
        }
        public IActionResult Index()
        {
            return View(_user.GetAll());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(User user)
        {
            ServiceResult result = _user.Create(user);
            TempData["Success"] = result.success;
            TempData["Message"] = result.message;
            return View(user);
        }
        public IActionResult Edit(int id)
        {
            return View(_user.GetById(id));
        }
        [HttpPost]
        public IActionResult Edit(User user)
        {
            ServiceResult result = _user.Update(user);
            TempData["Success"] = result.success;
            TempData["Message"] = result.message;
            return View(user);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            ServiceResult result = _user.Delete(id);
            TempData["Success"] = result.success;
            TempData["Message"] = result.message;
            return RedirectToAction("Index");
        }

    }
}
