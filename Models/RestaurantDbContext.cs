using BackOffice.Areas.Setting.Models;
using Microsoft.EntityFrameworkCore;

namespace BackOffice.Models
{
    public class RestaurantDbContext:DbContext
    {
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options) { }
        public DbSet<Role> roles  { get; set; }
    }
}
