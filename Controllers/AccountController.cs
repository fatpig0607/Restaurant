using BackOffice.Areas.Setting.Models;
using BackOffice.Areas.Setting.Models.Service.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BackOffice.Controllers
{
    public class AccountController : Controller
    {
        private IUserRepository _user;
        public AccountController(IUserRepository user)
        {
            _user = user;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        } 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(User user)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.LoginErr = "帳號、密碼欄位限制輸入有誤";
                return View(user);
            }
            var LoginUser = _user.ValidateLogin(user.user_name, user.password_hash);

            if (LoginUser == null)
            {
                ViewBag.LoginErr = "帳號、密碼有誤，或帳號已停用";
                return View(user);
            }
            else
            {
                ViewBag.LoginErr = "帳密驗證成功，目前尚未建立登入狀態";
                return View(user);
            }
        }
    }
}
