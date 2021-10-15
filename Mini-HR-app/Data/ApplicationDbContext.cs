using Microsoft.EntityFrameworkCore;
using Mini_HR_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mini_HR_app.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Person> People { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>()
                    .HasMany(x => x.Employees)
                    .WithMany(x => x.Companies)
                    .UsingEntity<CompanyEmployee>(
                        x => x.HasOne(e => e.Employee).WithMany(c => c.CompanyEmployees),
                        x => x.HasOne(e => e.Company).WithMany(p => p.CompanyEmployees));
        }
    }
}
