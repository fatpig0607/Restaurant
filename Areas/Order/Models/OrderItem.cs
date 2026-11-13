using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackOffice.Areas.Order.Models
{
    [Table("order_items")]
    public class OrderItem
    {
        [Key]
        public int order_item_id { get; set; }
        public string order_id { get; set; } = "";
        public int menu_id { get; set; }
        public string menu_name { get; set; } = "";
        public int price { get; set; }
        public int quantity { get; set; }
    }
}
