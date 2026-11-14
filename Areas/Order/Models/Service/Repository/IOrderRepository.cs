namespace BackOffice.Areas.Order.Models.Service.Repository
{
    public interface IOrderRepository
    {
        public IEnumerable<Order> GetAll();
        public IEnumerable<OrderItem> Detail(string id);
    }
}
