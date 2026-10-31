using BackOffice.Areas.Setting.Models.Service.Repository;
using BackOffice.Areas.Setting.Models.ViewModel;
using BackOffice.Models;

namespace BackOffice.Areas.Setting.Models.Service
{
    public class RoleService : IRoleRepository
    {
        private RestaurantDbContext _dbContext;
        public RoleService(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public ServiceResult Delete(int id)
        {
            var q = _dbContext.roles.FirstOrDefault(r => r.role_id == id);
            if (_dbContext.users.Any(r => r.role_id == q.role_id))
            {
                return new ServiceResult { success = false, message = "已有帳號綁定此角色，需先解除帳號綁定" };
            }
            else if (q != null)
            {
                _dbContext.roles.Remove(q);
                _dbContext.SaveChanges();
                return new ServiceResult { success = true, message = "成功刪除指定角色" };
            }
            else
            {
                return new ServiceResult { success = false, message = "無指定角色" };
            }
        }

        public IEnumerable<Role> GetAll()
        {
            var roles = _dbContext.roles.ToList();
            return roles;
        }

        public Role GetById(int id)
        {
            var q = _dbContext.roles.FirstOrDefault(r => r.role_id == id);
            return q;
        }

        public ServiceResult Create(Role role)
        {
            if (string.IsNullOrWhiteSpace(role.role_name) || string.IsNullOrWhiteSpace(role.description))
            {
                return new ServiceResult { success = false, message = "角色名稱或描述不得為空" };
            }
            else if (_dbContext.roles.Any(nr => nr.role_name == role.role_name.Trim()))
            {
                return new ServiceResult { success = false, message = "已有存在相同的角色名稱" };
            }

            var r = new Role
            {
                created_at = DateTime.UtcNow,
                updated_at = DateTime.UtcNow,
                role_name = role.role_name.Trim(),
                description = role.description.Trim(),
            };

            _dbContext.roles.Add(r);
            _dbContext.SaveChanges();
            return new ServiceResult { success = true, message = "角色新增成功" };
        }

        public ServiceResult Update(Role role)
        {
            if (string.IsNullOrWhiteSpace(role.role_name) || string.IsNullOrWhiteSpace(role.description))
            {
                return new ServiceResult { success = false, message = "角色名稱或描述不得為空" };
            }
            else if (_dbContext.roles.Any(nr => nr.role_name == role.role_name.Trim() && role.role_id != nr.role_id))
            {
                return new ServiceResult { success = false, message = "已有存在相同的角色名稱" };
            }
            var q = _dbContext.roles.FirstOrDefault(r => r.role_id == role.role_id);
            if (q != null)
            {
                q.description = role.description.Trim();
                q.role_name = role.role_name.Trim();
                q.updated_at = DateTime.UtcNow;
                _dbContext.SaveChanges();
                return new ServiceResult { success = true, message = "角色更新成功!" };
            }
            else
            {
                return new ServiceResult { success = false, message = "無此角色名稱" };
            }
        }

        public RolePermissionViewModel? GetPermissions(int roleId)
        {
            var role = _dbContext.roles.FirstOrDefault(r=>r.role_id==roleId);
            if (role == null)
            {
                return null;
            }
            var permissions = _dbContext.permissions.OrderBy(p=>p.permission_id).ToList();
            var selectedIds = _dbContext.roles_permissions.Where(r=>r.role_id==roleId).Select(rp=>rp.permission_id).ToList() ;
            return new RolePermissionViewModel
            {
                role_id = role.role_id,
                role_name = role.role_name,
                permissions = permissions,
                selected_permissions_ids = selectedIds
            };
        }

        public ServiceResult SavePermissions(int Roleid, List<int> selectedPermissionIds)
        {
            if (!_dbContext.roles.Any(r => r.role_id == Roleid))
            {
                return new ServiceResult { success = false, message = "無此角色ID" };
            }
            var originPermissions = _dbContext.roles_permissions.Where(rp => rp.role_id == Roleid).ToList();//原權限物件
            var PermissionsIds = selectedPermissionIds.Distinct().ToList();//新設定的權限清單
            var RemovePermissions = originPermissions.Where(rp =>!PermissionsIds.Contains(rp.permission_id)).ToList();//從舊權限裡，挑出新名單已經沒有的那些資料，整理成清單
            var AddPermissionsIds = PermissionsIds.Where(p=>!originPermissions.Any(rp=>rp.permission_id==p)).ToList();//從新名單挑；舊名單找不到，才留下來新增。

            var existingIds = _dbContext.permissions.Select(p => p.permission_id).ToList();
            if (PermissionsIds.Any(p => !existingIds.Contains(p)))
            {
                return new ServiceResult { success = false, message = "包含無效的權限" };
            }
            //檢查有沒有沒查詢權，但有操作權
            var selectedCodes = _dbContext.permissions.Where(p => PermissionsIds.Contains(p.permission_id))
                .Select(p=>p.permission_code).ToList();
            bool hasRoleOperation = selectedCodes.Contains("Role.Create") || selectedCodes.Contains("Role.Edit") || selectedCodes.Contains("Role.Delete");
            if (hasRoleOperation && !selectedCodes.Contains("Role.View"))
            {
                return new ServiceResult { success = false, message = "設定角色操作權時，需勾選角色查詢權" };
            }
            bool hasUserOperation = selectedCodes.Contains("User.Create") || selectedCodes.Contains("User.Edit") || selectedCodes.Contains("User.Delete");
            if (hasUserOperation && !selectedCodes.Contains("User.View"))
            {
                return new ServiceResult { success = false, message = "設定帳號操作權時，需勾選帳號查詢權" };
            }
            foreach (var RemovePermission in RemovePermissions)
            {
                _dbContext.roles_permissions.Remove(RemovePermission);
            }
            foreach (var AddPermissionsId in AddPermissionsIds)
            {
                RolePermission rp = new RolePermission
                {
                    role_id = Roleid,
                    permission_id=AddPermissionsId
                };
                _dbContext.roles_permissions.Add(rp);
            }
            _dbContext.SaveChanges();
            return new ServiceResult { success = true, message = "已成功保存權限" };
        }

    }
}
