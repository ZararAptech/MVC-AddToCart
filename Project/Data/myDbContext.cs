using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Data
{
    public class myDbContext : DbContext
    {
        public myDbContext() { }
        public myDbContext(DbContextOptions<myDbContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<ProductCart> Productcarts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-1NHQSUD\\SQLEXPRESS;Initial Catalog=practice;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        }
    }
}
