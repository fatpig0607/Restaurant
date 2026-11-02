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
            throw new NotImplementedException();

        }

        public ServiceResult Edit(MenuCategories menucategory)
        {
            throw new NotImplementedException();

        }

    }
}
