using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using PShop.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PShop
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Order> Order { get; set; }

        public DbSet<Employee> Employee { get; set; }

        public DbSet<Customer> Customer { get; set; }

        public DbSet<OrderedProduct> OrderedProduct { get; set; }

        public DbSet<Product> Product { get; set; }

        public string ConnectionString { get; }

        public ApplicationDbContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(this.ConnectionString);
        }
    }

}
