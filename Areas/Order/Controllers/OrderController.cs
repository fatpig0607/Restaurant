using BackOffice.Areas.Order.Models.Service.Repository;
using BackOffice.Areas.Setting.Models.Service.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace BackOffice.Areas.Order.Controllers
{
    [Authorize]
    [Area("Order")]
    public class OrderController : Controller
    {
        private IOrderRepository _order;
        private IUserRepository _user;
        public OrderController(IOrderRepository order,IUserRepository user)
        {
            _order = order;
            _user = user;
        }
        private List<string> GetPermissionCodes()
        {
            string? userIdText = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(int.TryParse(userIdText,out int userId))
            {
                return _user.GetPermissionCodes(userId);
            }
            else
            {
                return new List<string>();
            }

        }
        public IActionResult Index()
        {
            ViewBag.permissionCodes = GetPermissionCodes();
            if (ViewBag.permissionCodes.Contains("Order.View"))
            {
                return View(_order.GetAll());
            }
            else
            {
                return StatusCode(403);
            }
        }
        public IActionResult Detail(string id)
        {
            ViewBag.permissionCodes = GetPermissionCodes();
            if (ViewBag.permissionCodes.Contains("Order.Detail"))
            {
                var q = _order.Detail(id);
                return View(q);
            }
            else
            {
                return StatusCode(403);
            }
        }
    }
}
