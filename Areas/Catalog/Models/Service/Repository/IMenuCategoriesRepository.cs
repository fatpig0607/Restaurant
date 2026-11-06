using BackOffice.Models;
using System.Runtime.InteropServices;

namespace BackOffice.Areas.Catalog.Models.Service.Repository
{
    public interface IMenuCategoriesRepository
    {
        public IEnumerable<MenuCategories> GetAll();
        public ServiceResult Create(MenuCategories menucategory);
        public ServiceResult Edit(MenuCategories menucategory);
        public ServiceResult Delete(int id);
        public MenuCategories? GetById(int id);
    }
}
