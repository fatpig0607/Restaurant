using BackOffice.Areas.Setting.Models.Service.Repository;
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
            if (q != null)
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
    }
}
