namespace BackOffice.Areas.Setting.Models.ViewModel
{
    public class RolePermissionViewModel
    {
        public int role_id { get; set; }
        public string role_name { get; set; } = "";
        public List<Permission> permissions { get; set; } = new();
        public List<int> selected_permissions_ids { get; set; } = new();

    }
}
