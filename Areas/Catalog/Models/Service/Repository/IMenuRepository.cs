using BackOffice.Areas.Catalog.Models.ViewModel;
using BackOffice.Models;

namespace BackOffice.Areas.Catalog.Models.Service.Repository
{
    public interface IMenuRepository
    {
        public IEnumerable<MenuViewModel> GetMenuList(int id);
        public IEnumerable<MenuCategories> GetMenuCategories();
        public ServiceResult Create(Menu menu, IFormFile? img);
        public Menu GetById(int id);
        public ServiceResult Edit(Menu menu, IFormFile? img, bool removeImage);
        public ServiceResult Delete(int id);
    }
}
