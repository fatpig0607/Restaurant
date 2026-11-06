using BackOffice.Areas.Catalog.Models.Service.Repository;
using BackOffice.Areas.Catalog.Models.ViewModel;
using BackOffice.Models;

namespace BackOffice.Areas.Catalog.Models.Service
{
    public class MenuService : IMenuRepository
    {
        private RestaurantDbContext _dbContext;
        private IWebHostEnvironment _environment;
        public MenuService(RestaurantDbContext dbContext,IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _environment = environment;
        }
        public IEnumerable<MenuViewModel> GetMenuList(int id)
        {
            var q = from m in _dbContext.menu
                    join mc in _dbContext.menu_categories
                    on m.category_id equals mc.category_id
                    select new MenuViewModel
                    {
                        menu_id = m.menu_id,
                        category_id = m.category_id,
                        category_name = mc.category_name,
                        menu_name = m.menu_name,
                        menu_description = m.menu_description,
                        menu_img = m.menu_img,
                        price = m.price,
                        status = m.status,
                        sort = m.sort,
                        created_at = m.created_at,
                        updated_at = m.updated_at
                    };
            if (id!=0 &&_dbContext.menu_categories.Any(mc=>mc.category_id==id))
            { 
                q = q.Where(m => m.category_id == id);
            }
            return  q.OrderBy(m => m.sort).ThenBy(m => m.menu_id).ToList();
        }
        public IEnumerable<MenuCategories> GetMenuCategories()
        {
            var q = _dbContext.menu_categories.OrderBy(mc=>mc.sort).ThenBy(mc=>mc.category_id).ToList();
            return q;
        }
        public ServiceResult Create(Menu menu, IFormFile? img)
        {
            if (!_dbContext.menu_categories.Any(mc => mc.category_id == menu.category_id))
            {
                return new ServiceResult { success = false, message = "餐點分類不存在" };
            }
            else if (string.IsNullOrWhiteSpace(menu.menu_name) || menu.menu_name.Length>50)
            {
                return new ServiceResult { success=false,message="餐點名稱需為1~50字"};
            }else if (menu.menu_description!=null && menu.menu_description.Length>100)
            {
                return new ServiceResult { success = false, message = "餐點描述需為100字以下" };
            }else if (menu.status!=0 && menu.status!=1)
            {
                return new ServiceResult { success = false, message = "餐點狀態有誤" };
            }else if (menu.price < 0)
            {
                return new ServiceResult { success = false, message = "價格需大於等於0" };
            }else if(menu.sort > 999 || menu.sort < 0)
            {
                return new ServiceResult { success = false, message = "排序需為0~999" };
            }
            string? imagePath = null;
            if (img != null)
            {
                if(img.Length==0 || img.Length > 2 * 1024 * 1024)
                {
                    return new ServiceResult { success = false, message = "圖片不可為空檔案，且檔案大小不得超過2MB" };
                }
                string extension = Path.GetExtension(img.FileName).ToLower();
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
                if (!allowedExtensions.Contains(extension))
                {
                    return new ServiceResult { success = false, message = "圖片僅支援jpg、jpeg、png、webp" };
                }
                string uploadsfile = Path.Combine(_environment.WebRootPath, "uploads", "menu");//準備存圖片的資料夾路徑
                Directory.CreateDirectory(uploadsfile);//依照路徑建立資料夾
                string fileName= Guid.NewGuid().ToString("N")+extension;//準備圖片名稱
                string filePath = Path.Combine(uploadsfile, fileName);//準備圖片路徑
                using (var stream = new FileStream(filePath, FileMode.CreateNew))
                {
                    img.CopyTo(stream);
                }
                
                    imagePath = "/uploads/menu/" + fileName;
            }

            Menu NewMenu = new Menu
            {
                category_id = menu.category_id,
                menu_name = menu.menu_name.Trim(),
                menu_description = menu.menu_description,
                menu_img=imagePath,
                price = menu.price,
                status = menu.status,
                sort = menu.sort,
                created_at = DateTime.Now
            };
            _dbContext.menu.Add(NewMenu);
            _dbContext.SaveChanges();
            return new ServiceResult { success = true, message = "餐點新增成功" };

        }
        public Menu GetById(int id)
        {
            var q = _dbContext.menu.FirstOrDefault(m=>m.menu_id==id);
            return q;
        }
        public ServiceResult Edit(Menu menu, IFormFile? img,bool removeImage)
        {
            if (!_dbContext.menu_categories.Any(mc => mc.category_id == menu.category_id))
            {
                return new ServiceResult { success = false, message = "餐點分類不存在" };
            }
            else if (string.IsNullOrWhiteSpace(menu.menu_name) || menu.menu_name.Length > 50)
            {
                return new ServiceResult { success = false, message = "餐點名稱需為1~50字" };
            }
            else if (menu.menu_description != null && menu.menu_description.Length > 100)
            {
                return new ServiceResult { success = false, message = "餐點描述需為100字以下" };
            }
            else if (menu.status != 0 && menu.status != 1)
            {
                return new ServiceResult { success = false, message = "餐點狀態有誤" };
            }
            else if (menu.price < 0)
            {
                return new ServiceResult { success = false, message = "價格需大於等於0" };
            }
            else if (menu.sort > 999 || menu.sort < 0)
            {
                return new ServiceResult { success = false, message = "排序需為0~999" };
            }
            var q = _dbContext.menu.FirstOrDefault(m => m.menu_id == menu.menu_id);
            if (q == null)
            {
                return new ServiceResult { success = false, message = "無此餐點" };
            }
            if (removeImage)
            {
                q.menu_img = null;
            }
            if (img != null)
            {
                string extension = Path.GetExtension(img.FileName).ToLower();
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
                if(img.Length==0|| img.Length > 2 * 1024 * 1024)
                {
                    return new ServiceResult { success = false, message = "圖片不可為空檔案，且檔案大小不得超過2MB" };
                }else if (!allowedExtensions.Contains(extension))
                {
                    return new ServiceResult { success = false, message = "圖片僅支援jpg、jpeg、png、webp" };
                }
                string uploadFiles = Path.Combine(_environment.WebRootPath, "uploads", "menu");
                Directory.CreateDirectory(uploadFiles);
                string imgName=Guid.NewGuid().ToString("N")+extension;
                string uploadPath=Path.Combine(uploadFiles, imgName);
                using (var stream = new FileStream(uploadPath, FileMode.CreateNew))
                {
                    img.CopyTo(stream);
                    q.menu_img = "/uploads/menu/" + imgName;
                }
            }


            q.category_id = menu.category_id;
            q.menu_name = menu.menu_name;
            q.menu_description = menu.menu_description;
            q.status = menu.status;
            q.sort = menu.sort;
            q.price=menu.price;
            q.updated_at = DateTime.Now;
            _dbContext.SaveChanges();
            return new ServiceResult { success = true, message = "餐點更新成功" };


        }
        public ServiceResult Delete(int id)
        {
            var q = _dbContext.menu.FirstOrDefault(m => m.menu_id == id);
            if (q != null)
            {
                _dbContext.menu.Remove(q);
                _dbContext.SaveChanges();
                return new ServiceResult { success = true, message = "成功刪除指定餐點" };
            }
            else
            {
                return new ServiceResult { success = false, message = "無指定餐點" };
            }
        }
    }
}
