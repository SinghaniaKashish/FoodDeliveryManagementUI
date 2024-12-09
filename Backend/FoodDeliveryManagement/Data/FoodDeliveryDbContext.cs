using FoodDeliveryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryManagement.Data
{
    public class FoodDeliveryDbContext : DbContext
    {
        public FoodDeliveryDbContext(DbContextOptions<FoodDeliveryDbContext> options) : base(options) { }

        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<IngredientUsage> IngredientUsages { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<CartItem> Cart { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant>()
            .HasMany(r => r.MenuItems)
            .WithOne(m => m.Restaurant)
            .HasForeignKey(m => m.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MenuItem>()
                .HasMany(m => m.Ingredients)
                .WithOne()
                .HasForeignKey(i => i.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderDetail>()
                .Property(od => od.OrderDetailId) 
                .ValueGeneratedOnAdd();

            base.OnModelCreating(modelBuilder);
        }

    }
}
