using System.ComponentModel.DataAnnotations;

namespace BackOffice.Areas.Setting.Models
{
    public class Role
    {
        [Key]
        public int role_id { get; set; }
        [Required]
        [Display(Name = "角色名稱", Description = "就只是角色名稱")]
        public string role_name { get; set; }
        [Display(Name = "角色描述", Description = "就只是角色描述")]
        public string description { get; set; }
        [Display(Name = "創建時間")]
        public DateTime created_at { get; set; }
        [Display(Name = "最後更新時間")]
        public DateTime updated_at { get; set; }
    }
}
