using BackOffice.Areas.Order.Models.Request;
using BackOffice.Areas.Order.Models.Service;
using BackOffice.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackOffice.Areas.Order.Controllers
{
    [ApiController]
    [Route("api")]
    public class OrderApiController : ControllerBase
    {
        private OrderApiService _orderservice;
        public OrderApiController(OrderApiService orderservice)
        {
            _orderservice = orderservice;
        }
        [HttpPost("order")]
        public IActionResult Create([FromBody] CreateOrderRequest request)
        {
            ServiceResult result = _orderservice.Create(request);

            if (!result.success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
