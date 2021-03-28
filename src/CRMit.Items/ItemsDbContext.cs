using CRMit.Items.Models;
using Microsoft.EntityFrameworkCore;

namespace CRMit.Items
{
    public class ItemsDbContext : DbContext
    {
        public ItemsDbContext(DbContextOptions<ItemsDbContext> dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Item>()
                .HasIndex(c => c.Name)
                .IsUnique();
        }
    }
}
