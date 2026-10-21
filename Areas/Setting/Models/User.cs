using System.ComponentModel.DataAnnotations;

namespace BackOffice.Areas.Setting.Models
{
    public class User
    {
        [Key]
        public int user_id { get; set; }
        [Required]
        [Display(Name ="帳號")]
        public string user_name { get; set; }
        [Required]
        [Display(Name = "密碼")]
        public string password_hash { get; set; }
        [Display(Name = "狀態")]
        public byte status { get; set; }
        [Display(Name = "創建時間")]
        public DateTime created_at { get; set; }
        [Display(Name = "最後更新時間")]
        public DateTime updated_at { get; set; }
        [Display(Name = "最後登入時間")]
        public DateTime? last_login_at { get; set; }
        [Display(Name ="角色名稱")]
        public int? role_id { get; set; }
    }
}
