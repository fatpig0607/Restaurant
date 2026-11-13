using BackOffice.Areas.Catalog.Models;
using BackOffice.Areas.Order.Models;
using BackOffice.Areas.Setting.Models;
using Microsoft.EntityFrameworkCore;

namespace BackOffice.Models
{
    public class RestaurantDbContext:DbContext
    {
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options) { }
        public DbSet<Role> roles  { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Permission> permissions { get; set; }
        public DbSet<RolePermission> roles_permissions { get; set; }
        public DbSet<MenuCategories> menu_categories { get; set; }
        public DbSet<Menu> menu {  get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderItem> order_items { get; set; }
    }
}
