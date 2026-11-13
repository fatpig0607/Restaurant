using BackOffice.Areas.Catalog.Models.Service.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BackOffice.Areas.Catalog.Controllers
{
    [ApiController]
    [Route("api")]
    public class MenuApiController : ControllerBase
    {
        private IMenuRepository _menu;
        private IMenuCategoriesRepository _menucategory;
        public MenuApiController(IMenuRepository menu, IMenuCategoriesRepository menucategory)
        {
            _menu = menu;
            _menucategory = menucategory;
        }
        [HttpGet("menu")]
        public IActionResult GetMenu()
        {
            var q = _menu.GetMenuList(0).Select(item => new
            {
                id = item.menu_id,
                category = item.category_name,
                name = item.menu_name,
                price = item.price,
                status=item.status,
                sort = item.sort,
                imgurl = item.menu_img
            }).ToList();

            return Ok(q);
        }
        [HttpGet("menucategories")]
        public IActionResult GetMenuCategories()
        {
            var q = _menucategory.GetAll();
            return Ok(q);
        }
    }
}
