using Microsoft.EntityFrameworkCore;
using SellingKoi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace SellingKoi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options) { }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Farm> Farms { get; set; }
        public DbSet<KOI> KOIs { get; set; }
        public DbSet<Models.Route> Routes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderShorten> OrtherShortens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Định nghĩa quan hệ 1-nhiều giữa Farm và KoiProduct
            modelBuilder.Entity<Farm>()
                .HasMany(f => f.KOIs)
                .WithOne(k => k.Farm)
                .HasForeignKey(k => k.FarmID);
            //nhieu farm nhieu route
            modelBuilder.Entity<Farm>()
                .HasMany(f => f.Routes)
                .WithMany(r => r.Farms);


            // Định nghĩa quan hệ 1-1 giữa Account và Cart
            modelBuilder.Entity<Account>()
                .HasOne(a => a.Cart) // Giả sử bạn đã thêm thuộc tính Cart trong Account
                .WithOne(c => c.Account) // Giả sử bạn đã thêm thuộc tính Account trong Cart
                .HasForeignKey<Cart>(c => c.Id);

            //nhieu cart nhieu koi
            modelBuilder.Entity<Cart>()
               .HasMany(c => c.KOIs)
               .WithMany(k => k.Carts);

            //1 route - nhieu cart
            modelBuilder.Entity<Models.Route>()
                .HasMany(f => f.Carts)
                .WithOne(k => k.Route)
                .HasForeignKey(k => k.RouteId);

            //nhieu order nhieu route
            // Định nghĩa quan hệ 1-nhiều giữa Route và Order

            modelBuilder.Entity<Models.Route>()
                .HasMany(r => r.Orders)
                .WithOne(o => o.Route)
                .HasForeignKey(o => o.RouteId);
        }

    }
}
