using BackOffice.Areas.Setting.Models;
using BackOffice.Areas.Setting.Models.Service.Repository;
using BackOffice.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            return View(_user.GetUserList());
        }
        public IActionResult Create()
        {
            var roles=_user.GetRoles();
            ViewBag.RoleList = new SelectList(roles, "role_id", "role_name");
            return View();
        }
        [HttpPost]
        public IActionResult Create(User user)
        {
            var roles = _user.GetRoles();
            ViewBag.RoleList = new SelectList(roles,"role_id","role_name");//讓了讓post送出時，背景的角色選單維持一致


            ServiceResult result = _user.Create(user);
            TempData["Success"] = result.success;
            TempData["Message"] = result.message;
            return View(user);
        }
        public IActionResult Edit(int id)
        {
            var roles = _user.GetRoles();
            ViewBag.RoleList = new SelectList(roles, "role_id", "role_name");
            return View(_user.GetById(id));
        }
        [HttpPost]
        public IActionResult Edit(User user)
        {
            var roles = _user.GetRoles();
            ViewBag.RoleList = new SelectList(roles, "role_id", "role_name");//讓了讓post送出時，背景的角色選單維持一致


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
