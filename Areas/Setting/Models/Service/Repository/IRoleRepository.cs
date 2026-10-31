using BackOffice.Areas.Setting.Models.ViewModel;
using BackOffice.Models;
using System.Data;

namespace BackOffice.Areas.Setting.Models.Service.Repository
{
    public interface IRoleRepository
    {
        IEnumerable<Role> GetAll();
        Role GetById(int id);
        ServiceResult Create(Role role);
        ServiceResult Update(Role role);
        ServiceResult Delete(int id);
        RolePermissionViewModel? GetPermissions(int roleId);
        ServiceResult SavePermissions(int Roleid, List<int> selectedPermissionIds);

    }

}
