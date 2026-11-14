using BackOffice.Areas.Order.Models.Service.Repository;
using BackOffice.Models;

namespace BackOffice.Areas.Order.Models.Service
{
    public class OrderService : IOrderRepository
    {
        private RestaurantDbContext _dbContext;
        public OrderService(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public IEnumerable<Order> GetAll()
        {
            var q = _dbContext.orders.OrderByDescending(o=>o.created_at).ToList();
            return q;
        }
        public IEnumerable<OrderItem> Detail(string id)
        {
            var q = _dbContext.order_items.Where(oi => oi.order_id == id).ToList();
            return q;
        }
    }
}
