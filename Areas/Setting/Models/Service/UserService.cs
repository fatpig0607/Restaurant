using BackOffice.Areas.Setting.Models.Service.Repository;
using BackOffice.Models;

namespace BackOffice.Areas.Setting.Models.Service
{
    public class UserService : IUserRepository
    {
         private RestaurantDbContext _dbContext;
        public UserService(RestaurantDbContext dbContext) 
        { 
            _dbContext = dbContext;
        }
        public ServiceResult Create(User user)
        {
            if(string.IsNullOrWhiteSpace(user.user_name) || string.IsNullOrWhiteSpace(user.password_hash))
            {
                return new ServiceResult { success = false, message = "帳號或密碼不得為空" };
            }
            else if(_dbContext.users.Any(u=>u.user_name.Trim()==user.user_name.Trim())){
                return new ServiceResult { success = false, message = "已存在相同的帳號" };
            }
            var u = new User
            {
                user_name = user.user_name.Trim(),
                password_hash = user.password_hash,
                status = 1,
                created_at = DateTime.Now,
                updated_at = DateTime.Now,
            };
            _dbContext.users.Add(u);
            _dbContext.SaveChanges();
            return new ServiceResult { success = true, message = "帳號新增成功" };
        }

        public ServiceResult Delete(int id)
        {
            var u = _dbContext.users.FirstOrDefault(u=>u.user_id==id);
            if (u != null)
            {
                _dbContext.users.Remove(u);
                _dbContext.SaveChanges();
                return new ServiceResult { success = true, message = "成功刪除指定帳號" };
            }
            else
            {
                return new ServiceResult { success = false, message = "無指定帳號" };
            }
        }

        public IEnumerable<User> GetAll()
        {
            var users = _dbContext.users.ToList();
            return users;
        }

        public User GetById(int id)
        {
            var u = _dbContext.users.FirstOrDefault(u => u.user_id == id);
            return u;
        }

        public ServiceResult Update(User user)
        {
            if (string.IsNullOrWhiteSpace(user.password_hash))
            {
                return new ServiceResult { success = false, message = "密碼不得為空" };
            }
            
            var u = _dbContext.users.FirstOrDefault(u => u.user_id == user.user_id);
            if (u != null)
            {
                u.password_hash= user.password_hash;
                u.status=user.status;
                u.updated_at = DateTime.UtcNow;
                _dbContext.SaveChanges();
                return new ServiceResult { success = true, message = "帳號更新成功" };
            }
            else
            {
                return new ServiceResult { success = false, message = "無指定帳號" };
            }
        }
    }
}
