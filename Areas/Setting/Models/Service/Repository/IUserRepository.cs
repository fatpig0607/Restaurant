using BackOffice.Models;

namespace BackOffice.Areas.Setting.Models.Service.Repository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User GetById(int id);
        ServiceResult Create(User user);
        ServiceResult Update(User user);
        ServiceResult Delete(int id);
    }
}
