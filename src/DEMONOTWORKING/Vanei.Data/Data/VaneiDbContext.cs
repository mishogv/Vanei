namespace Vanei.Data.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using Models.Models;

    public class VaneiDbContext : IdentityDbContext<WebAppUser>
    {
        public VaneiDbContext(DbContextOptions<VaneiDbContext> options)
            : base(options)
        {
        }

        public VaneiDbContext()
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<WareHouse> WareHouses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Company>()
                        .HasOne(x => x.Owner)
                        .WithOne(x => x.Company)
                        .HasForeignKey<Company>(x => x.OwnerId);

            base.OnModelCreating(builder);
        }
    }
}
