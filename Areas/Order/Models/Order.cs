using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackOffice.Areas.Order.Models
{
    [Table("orders")]
    public class Order
    {
        [Key]
        public string order_id { get; set; } = "";
        public int total_price { get; set; }
        public DateTime created_at { get; set; }
    }
}
