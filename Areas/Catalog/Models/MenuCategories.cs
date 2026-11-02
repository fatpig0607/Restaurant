using System.ComponentModel.DataAnnotations;

namespace BackOffice.Areas.Catalog.Models
{
    public class MenuCategories
    {
        [Key]
        [Display(Name = "分類編號")]
        public int category_id { get; set; }
        [Display(Name = "分類名稱")]
        public string category_name { get; set; } = "";
        [Display(Name = "分類排序")]
        public int sort {  get; set; }
    }
}
