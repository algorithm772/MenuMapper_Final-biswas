using Microsoft.EntityFrameworkCore;

namespace MenuMapper.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // These DbSets represent your actual SQL tables
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Allergen> Allergens { get; set; }
        public DbSet<MenuItemAllergen> MenuItemAllergens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MenuItem>().Property(m => m.Price).HasPrecision(18, 2);

            // 1. Define the composite primary key for the bridge table
            modelBuilder.Entity<MenuItemAllergen>()
                .HasKey(ma => new { ma.MenuItemId, ma.AllergenId });

            // 2. Map the relationship from the Bridge to the MenuItem
            modelBuilder.Entity<MenuItemAllergen>()
                .HasOne(ma => ma.MenuItem)
                .WithMany(m => m.MenuItemAllergens)
                .HasForeignKey(ma => ma.MenuItemId);

            // 3. Map the relationship from the Bridge to the Allergen
            modelBuilder.Entity<MenuItemAllergen>()
                .HasOne(ma => ma.Allergen)
                .WithMany(a => a.MenuItemAllergens)
                .HasForeignKey(ma => ma.AllergenId);
        }
    }
}