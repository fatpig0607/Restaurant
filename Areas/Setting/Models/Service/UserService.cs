using BackOffice.Areas.Setting.Models.Service.Repository;
using BackOffice.Areas.Setting.Models.ViewModel;
using BackOffice.Models;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace BackOffice.Areas.Setting.Models.Service
{
    public class UserService : IUserRepository
    {
        private RestaurantDbContext _dbContext;
        private IPasswordHasher<User> _passwordhasher;
        public UserService(RestaurantDbContext dbContext,IPasswordHasher<User> passwordHasher) 
        { 
            _dbContext = dbContext;
            _passwordhasher = passwordHasher;
        }
        public ServiceResult Create(User user)
        {
            if(string.IsNullOrWhiteSpace(user.user_name) || string.IsNullOrWhiteSpace(user.password_hash))
            {
                return new ServiceResult { success = false, message = "帳號或密碼不得為空" };
            }
            else if(_dbContext.users.Any(u=>u.user_name.Trim()==user.user_name.Trim())){
                return new ServiceResult { success = false, message = "已存在相同的帳號" };
            }else if (!_dbContext.roles.Any(r=>r.role_id==user.role_id))
            {
                return new ServiceResult { success = false, message = "不存在的角色" };
            }
            var u = new User
            {
                user_name = user.user_name.Trim(),
                //password_hash = user.password_hash,
                status = 1,
                created_at = DateTime.Now,
                updated_at = DateTime.Now,
                role_id=user.role_id
            };
            u.password_hash = _passwordhasher.HashPassword(u, user.password_hash);
            //HashPassword(要處理的使用者物件, 原始密碼)
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

        //public IEnumerable<User> GetAll()
        //{
        //    var users = _dbContext.users.ToList();
        //    return users;
        //}

        public User GetById(int id)
        {
            var u = _dbContext.users.FirstOrDefault(u => u.user_id == id);
            return u;
        }

        public ServiceResult Update(User user)
        {
            if (!_dbContext.roles.Any(r=>r.role_id==user.role_id))
            {
                return new ServiceResult { success = false, message = "不存在的角色" };
            }
            
            var u = _dbContext.users.FirstOrDefault(u => u.user_id == user.user_id);
            
            if (u != null)
            {
                if (!string.IsNullOrWhiteSpace(user.password_hash))
                {
                    if (!Regex.IsMatch(user.password_hash,@"\A[!-~]{4,10}\z"))
                    {
                        return new ServiceResult { success = false, message = "密碼需在4~10位數" };
                    }
                    u.password_hash = _passwordhasher.HashPassword(u, user.password_hash);
                }

                    u.status=user.status;
                    u.updated_at = DateTime.Now;
                    u.role_id=user.role_id;
                    _dbContext.SaveChanges();
                    return new ServiceResult { success = true, message = "帳號更新成功" };
                }
                else
                {
                    return new ServiceResult { success = false, message = "無指定帳號" };
                }
        }
        public IEnumerable<Role> GetRoles()
        {
            return _dbContext.roles.ToList();
        }
        public IEnumerable<UserIndexViewModel> GetUserList()
        {
            var users = from u in _dbContext.users
                        join r in _dbContext.roles
                        on u.role_id equals r.role_id
                        select new UserIndexViewModel
                        {
                            user_id=u.user_id,
                            user_name = u.user_name,
                            status = u.status,
                            created_at = u.created_at,
                            updated_at = u.updated_at,
                            last_login_at = u.last_login_at,
                            role_id = u.role_id,
                            role_name = r.role_name
                        };
            return users.ToList();
        }

        public User? ValidateLogin(string username, string password)
        {
            var user = _dbContext.users.FirstOrDefault(u => u.user_name == username.Trim() && u.status == 1);
            if (user == null)
            {
                return null;
            }
            var result = _passwordhasher.VerifyHashedPassword(user, user.password_hash, password);
            if (result ==PasswordVerificationResult.Failed)
            {
                return null;
            }
            else
            {
                user.last_login_at=DateTime.Now;
                _dbContext.SaveChanges();
                return user;
            }
        }
    }
}
