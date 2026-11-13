namespace BackOffice.Areas.Order.Models.Service.Repository
{
    public interface IOrderRepository
    {
        public IEnumerable<Order> GetAll();
    }
}
