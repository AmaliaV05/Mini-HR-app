using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mini_HR_app.Models;

namespace Mini_HR_app.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>()
                    .HasMany(x => x.Employees)
                    .WithMany(x => x.Companies)
                    .UsingEntity<CompanyEmployee>(
                        x => x.HasOne(e => e.Employee).WithMany(c => c.CompanyEmployees),
                        x => x.HasOne(e => e.Company).WithMany(p => p.CompanyEmployees));

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }
    }
}
