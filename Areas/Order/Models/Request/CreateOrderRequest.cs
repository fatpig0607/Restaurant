namespace BackOffice.Areas.Order.Models.Request
{
    public class CreateOrderRequest
    {
        public List<OrderItemRequest> items { get; set; } = new();
    }
}
