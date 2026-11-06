using BackOffice.Areas.Catalog.Models.Service.Repository;
using BackOffice.Models;

namespace BackOffice.Areas.Catalog.Models.Service
{
    public class MenuCategoriesService : IMenuCategoriesRepository
    {
        private RestaurantDbContext _dbContext;
        public MenuCategoriesService(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public System.Collections.Generic.IEnumerable<MenuCategories> GetAll()
        {
            var q = _dbContext.menu_categories.OrderBy(c=>c.sort).ThenBy(c=>c.category_id).ToList();
            return (q);    
        }

        public ServiceResult Create(MenuCategories menucategory)
        {
            if (string.IsNullOrWhiteSpace(menucategory.category_name) || menucategory.category_name.Trim().Length>50)
            {
                return new ServiceResult { success = false , message="分類名稱需為1~50字"};
            }
            if(menucategory.sort<0 || menucategory.sort > 999)
            {
                return new ServiceResult { success = false , message="排序需為0~999"};
            }
            if (_dbContext.menu_categories.Any(mc => mc.category_name == menucategory.category_name.Trim()))
            {
                return new ServiceResult { success = false , message = "分類名稱已存在" };
            }
            MenuCategories mc = new MenuCategories
            {
                category_name = menucategory.category_name.Trim(),
                sort = menucategory.sort
            };
            _dbContext.menu_categories.Add(mc);
            _dbContext.SaveChanges();
            return new ServiceResult { success = true, message = "分類新增成功" };
        }

        public ServiceResult Delete(int id)
        {
            var q = _dbContext.menu_categories.FirstOrDefault(mc => mc.category_id == id);
            if (q == null)
            {
                return new ServiceResult { success = false, message = "無指定分類編號" };
            }
            else
            {
                _dbContext.menu_categories.Remove(q);
                _dbContext.SaveChanges();
                return new ServiceResult { success = true, message = "成功刪除指定餐點分類" };
            }

        }

        public ServiceResult Edit(MenuCategories menucategory)
        {
            if(string.IsNullOrWhiteSpace(menucategory.category_name) || menucategory.category_name.Trim().Length > 50)
            {
                return new ServiceResult { success = false, message = "分類名稱需為1~50字" };
            }
            if (menucategory.sort < 0 || menucategory.sort > 999)
            {
                return new ServiceResult { success = false, message = "排序需為0~999" };
            }
            if(_dbContext.menu_categories.Any(mc=>mc.category_name==menucategory.category_name.Trim() && mc.category_id != menucategory.category_id))
            {
                return new ServiceResult { success = false, message = "已有相同的餐點分類" };
            }

            var q = _dbContext.menu_categories.FirstOrDefault(mc => mc.category_id == menucategory.category_id);

            if (q ==null)
            {
                return new ServiceResult { success = false, message = "無此餐點分類名稱" };
            }
            q.category_name = menucategory.category_name.Trim();
            q.sort = menucategory.sort;
            _dbContext.SaveChanges();
            return new ServiceResult { success = true, message = "餐點分類名稱更新成功" };
        }
        public MenuCategories? GetById(int id)
        {
            var q = _dbContext.menu_categories.FirstOrDefault(mc => mc.category_id == id);
            return q;
        }

    }
}
