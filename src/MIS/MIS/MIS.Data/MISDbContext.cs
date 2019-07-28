namespace MIS.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using Models;

    public class MISDbContext : IdentityDbContext<MISUser>
    {
        public MISDbContext(DbContextOptions<MISDbContext> options)
            : base(options)
        {
        }
        public MISDbContext()
        {
        }

        public DbSet<Product> Products { get; set; } 

        public DbSet<Message> Messages { get; set; } 

        public DbSet<Receipt> Receipts { get; set; } 

        public DbSet<ReceiptProduct> ReceiptProducts { get; set; } 

        public DbSet<Report> Reports { get; set; } 

        public DbSet<ReceiptReport> ReceiptReports { get; set; } 

        public DbSet<Category> Categories { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<WareHouse> WareHouses { get; set; }

        public DbSet<Invitation> Invitations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MISUser>()
                   .HasOne(x => x.Company)
                   .WithMany(x => x.Employees)
                   .HasForeignKey(x => x.CompanyId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<ReceiptProduct>()
                   .HasOne(x => x.Product)
                   .WithMany(x => x.ReceiptProducts);

            builder.Entity<ReceiptProduct>()
                   .HasOne(x => x.Receipt)
                   .WithMany(x => x.ReceiptProducts);

            builder.Entity<ReceiptReport>()
                   .HasOne(x => x.Receipt)
                   .WithMany(x => x.ReceiptReports)
                   .HasForeignKey(x => x.ReceiptId);

            builder.Entity<ReceiptReport>()
                   .HasOne(x => x.Report)
                   .WithMany(x => x.ReceiptReports)
                   .HasForeignKey(x => x.ReportId);

            base.OnModelCreating(builder);
        }
    }
}
