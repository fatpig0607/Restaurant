using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackOffice.Areas.Order.Models
{
    [Table("orders")]
    public class Order
    {
        [Key]
        [Display(Name ="訂單編號")]
        public string order_id { get; set; } = "";
        [Display(Name = "總價格")]
        public int total_price { get; set; }
        [Display(Name = "創建時間")]
        public DateTime created_at { get; set; }
    }
}
