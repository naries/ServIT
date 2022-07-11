using Microsoft.EntityFrameworkCore;
using ServIT.Models;

namespace ServIT.Data
{
    public class ServITDBContext : DbContext
    {
        public ServITDBContext(DbContextOptions<ServITDBContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }

    }
}