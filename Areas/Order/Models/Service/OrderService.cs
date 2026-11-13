using BackOffice.Areas.Order.Models.Request;
using BackOffice.Models;
using Microsoft.EntityFrameworkCore;

namespace BackOffice.Areas.Order.Models.Service
{
    public class OrderService
    {
        private RestaurantDbContext _dbContext;
        public OrderService(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public ServiceResult Create(CreateOrderRequest req)
        {
            if(req.items==null || req.items.Count==0)
            {
                return new ServiceResult { success = false, message = "請先加入餐點" };
            }
            int totalPrice = 0;
            List<OrderItem> orderItems = new();
            DateTime now = DateTime.Now;
            string order_Id = $"Order{now:yyyyMMddHHmmss}";
            if (_dbContext.orders.Any(o => o.order_id == order_Id))
            {
                return new ServiceResult { success = false, message = "請再重試一次" };
            }
            foreach (var item in req.items)
            {
                if (item.quantity <= 0)
                {
                    return new ServiceResult { success = false, message = "餐點數量不得小於等於0" };
                }
                var q = _dbContext.menu.FirstOrDefault(m => m.menu_id == item.menu_id);
                if (q == null)
                {
                    return new ServiceResult { success = false, message = "指定餐點不存在" };
                }
                else if (q.status != 1)
                {
                    return new ServiceResult { success = false, message = $"{q.menu_name} 目前停售中" };
                }
                totalPrice += q.price * item.quantity;
                orderItems.Add(new OrderItem
                {
                    order_id = order_Id,
                    menu_id = q.menu_id,
                    menu_name = q.menu_name,
                    price = q.price,
                    quantity = item.quantity
                });
            }
            var order = new Order
            {
                order_id = order_Id,
                total_price = totalPrice,
                created_at = now
            };
            try
            {
                _dbContext.orders.Add(order);
                _dbContext.order_items.AddRange(orderItems);
                _dbContext.SaveChanges();
                return new ServiceResult { success = true, message = "訂單已成功送出" };
            }
            catch(DbUpdateException)
            {
                return new ServiceResult { success = false, message = "送出訂單失敗，請稍後再試" };
            }
        }
    }
}
