using System.ComponentModel.DataAnnotations;

namespace BackOffice.Areas.Catalog.Models
{
    public class Menu
    {
        [Key]
        [Display(Name ="餐點編號")]
        public int menu_id {  get; set; }
        [Display(Name = "分類編號")]
        public int category_id {  get; set; }
        [Display(Name = "餐點名稱")]
        public string menu_name { get; set; } = "";
        [Display(Name = "餐點描述")]
        public string? menu_description { get; set; }
        [Display(Name = "餐點圖片")]
        public string? menu_img { get; set; }
        [Display(Name = "價格")]
        public int price {  get; set; }
        [Display(Name = "狀態")]
        public byte status { get; set; }
        [Display(Name = "餐點排序")]
        public int sort {  get; set; }
        [Display(Name = "創建時間")]
        public DateTime created_at {  get; set; }
        [Display(Name = "最後更新時間")]
        public DateTime? updated_at { get; set; }
    }
}
