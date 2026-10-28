using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackOffice.Areas.Setting.Models
{
    [Table("permissions")]
    public class Permission
    {
        [Key]
        public int permission_id { get; set; }
        public string permission_code { get; set; } = "";
        public string permission_name { get; set; } = "";
    }
}
