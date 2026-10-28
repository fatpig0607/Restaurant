using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackOffice.Areas.Setting.Models
{
    [Table("roles_permissions")]
    [PrimaryKey(nameof(role_id),nameof(permission_id))]
    public class RolePermission
    {
        public int role_id { get; set; }
        public int permission_id { get; set; }
    }
}
