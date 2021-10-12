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
        public DbSet<Company> Company { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Person> Person { get; set; }
    }
}
