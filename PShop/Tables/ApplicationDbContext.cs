using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PShop.Tables
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderedProduct> OrderedProducts { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public string ConnectionString { get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(this.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasIndex(e => e.Mail, "UQ__Customer__7A212904478434A7")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .HasColumnName("city");

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(50)
                    .HasColumnName("company_name");

                entity.Property(e => e.CompanyNumber).HasColumnName("company_number");

                entity.Property(e => e.CustomerName)
                    .HasMaxLength(50)
                    .HasColumnName("customer_name");

                entity.Property(e => e.FlatNumber).HasColumnName("flat_number");

                entity.Property(e => e.Mail)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("mail");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("phone_number")
                    .IsFixedLength();

                entity.Property(e => e.PostCode)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("post_code");

                entity.Property(e => e.Street)
                    .HasMaxLength(50)
                    .HasColumnName("street");

                entity.Property(e => e.StreetNumber).HasColumnName("street_number");

                entity.Property(e => e.Surname)
                    .HasMaxLength(50)
                    .HasColumnName("surname");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasIndex(e => e.Login, "UQ__Employee__7838F272C839EC6E")
                    .IsUnique();

                entity.HasIndex(e => e.Mail, "UQ__Employee__7A212904A017D421")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EmployeeBonus)
                    .HasColumnType("decimal(6, 2)")
                    .HasColumnName("employee_bonus")
                    .HasDefaultValueSql("((200.00))");

                entity.Property(e => e.EmployeeName)
                    .HasMaxLength(50)
                    .HasColumnName("employee_name");

                entity.Property(e => e.EmployeeSurname)
                    .HasMaxLength(50)
                    .HasColumnName("employee_surname");

                entity.Property(e => e.Login)
                    .HasMaxLength(50)
                    .HasColumnName("login");

                entity.Property(e => e.Mail)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("mail");

                entity.Property(e => e.Password)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("password")
                    .IsFixedLength();
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.DateOfPlacingTheOrder)
                    .HasColumnType("datetime")
                    .HasColumnName("date_of_placing_the_order");

                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");

                entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");

                entity.Property(e => e.OrderRealizationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("order_realization_date");

                entity.Property(e => e.ShippingDate)
                    .HasColumnType("datetime")
                    .HasColumnName("shipping_date");

                entity.Property(e => e.WhetherTheOrderFulfilled).HasColumnName("whether_the_order_fulfilled");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("id_customer");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("id_employee");
            });

            modelBuilder.Entity<OrderedProduct>(entity =>
            {
                entity.ToTable("Ordered_products");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderedProducts)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("id_order");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderedProducts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("id_product");
            });


            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AvailablePieces).HasColumnName("available_pieces");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.NetCatalogPrice)
                    .HasColumnType("decimal(6, 2)")
                    .HasColumnName("net_catalog_price");

                entity.Property(e => e.NetSellingPrice)
                    .HasColumnType("decimal(6, 2)")
                    .HasColumnName("net_selling_price");

                entity.Property(e => e.NetSellingWarehouse)
                    .HasColumnType("decimal(6, 2)")
                    .HasColumnName("net_selling_warehouse");

                entity.Property(e => e.ProducerId).HasColumnName("producer_id");

                entity.Property(e => e.ProductName)
                    .HasMaxLength(50)
                    .HasColumnName("product_name");

                entity.Property(e => e.VatRate).HasColumnName("VAT_rate");

            });


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

}
