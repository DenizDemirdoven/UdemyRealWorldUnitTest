using Microsoft.EntityFrameworkCore;

#nullable disable

namespace UdemyRealWorldUnitTest.Web.Models
{
    public partial class UdemyUnitTestDBContext : DbContext
    {
        public UdemyUnitTestDBContext()
        {
        }

        public UdemyUnitTestDBContext(DbContextOptions<UdemyUnitTestDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Color)
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsFixedLength(true);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            });

            //modelBuilder.Entity<Category>().HasData(new Category { Id = 1, Name = "Kalemler" }, new Category { Id = 2, Name = "Defterler" });



            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
