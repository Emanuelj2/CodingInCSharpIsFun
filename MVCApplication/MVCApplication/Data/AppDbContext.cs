using Microsoft.EntityFrameworkCore;
using MVCApplication.Models;

namespace MVCApplication.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public object InvoiceService { get; internal set; }

        //seed data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed initial data for Products
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Product 1", Description = "Description 1", Price = 10.0M },
                new Product { Id = 2, Name = "Product 2", Description = "Description 2", Price = 20.0M },
                new Product { Id = 3, Name = "Product 3", Description = "Description 3", Price = 30.0M }
            );

            // Seed initial data for Invoices
            modelBuilder.Entity<Invoice>().HasData(
                new Invoice { Id = 1, Number = "INV-001", Status = "Paid", IssueDate = new DateOnly(2023, 1, 1), DueDate = new DateOnly(2023, 1, 15), Service = "Web Design", UnitPrice = 500.00M, Quantity = 1, ClientName = "Client A", Email = "exmaple1@ecample.com", Phone = "123-456-7890", Address = "123 Main St" },
                new Invoice { Id = 2, Number = "INV-002", Status = "Unpaid", IssueDate = new DateOnly(2023, 2, 1), DueDate = new DateOnly(2023, 2, 15), Service = "SEO Services", UnitPrice = 300.00M, Quantity = 1, ClientName = "Client B", Email = "example2@example.com", Phone = "987-654-3210", Address = "456 Elm St" }
            );
        }

    }
}
