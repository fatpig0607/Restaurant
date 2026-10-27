using BackOffice.Areas.Setting.Models;
using BackOffice.Areas.Setting.Models.Service.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        public async Task<IActionResult> Login(User user)
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
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier,LoginUser.user_id.ToString()),
                    new Claim(ClaimTypes.Name,LoginUser.user_name)
                    //Claim 一項關於使用者的資料
                };
                var identity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                //ClaimsIdentity	把多項資料組成一個身分，包含驗證方式
                var principal = new ClaimsPrincipal(identity);
                //ClaimsPrincipal	 代表目前使用者
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                //await HttpContext.SignInAsync(使用哪種驗證方式, 登入的是誰);
                return RedirectToAction("Index","Home");
            }
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
