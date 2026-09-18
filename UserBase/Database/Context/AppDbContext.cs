using Microsoft.EntityFrameworkCore;
using UserBase.Model;

namespace UserBase.Database.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Gender> Genders => Set<Gender>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Gender>(e =>
            {
                e.ToTable("Genders");
                e.HasKey(g => g.Id);
                e.Property(g => g.Name).HasMaxLength(50).IsRequired();
            });

            mb.Entity<Employee>(e =>
            {
                e.ToTable("Employees");
                e.HasKey(x => x.Id);
                e.Property(x => x.Surname).HasMaxLength(100).IsRequired();
                e.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
                e.Property(x => x.Patronymic).HasMaxLength(100);
                e.Property(x => x.DateOfBirth).IsRequired();
                e.HasOne(x => x.Gender)
                    .WithMany()
                    .HasForeignKey(x => x.GenderId)
                    .OnDelete(DeleteBehavior.Restrict); // совместимость с Access
            });
        }
    }
}
