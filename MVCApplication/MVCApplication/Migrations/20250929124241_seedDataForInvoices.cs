using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MVCApplication.Migrations
{
    /// <inheritdoc />
    public partial class seedDataForInvoices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Invoices",
                columns: new[] { "Id", "Address", "ClientName", "DueDate", "Email", "IssueDate", "Number", "Phone", "Quantity", "Service", "Status", "UnitPrice" },
                values: new object[,]
                {
                    { 1, "123 Main St", "Client A", new DateOnly(2023, 1, 15), "exmaple1@ecample.com", new DateOnly(2023, 1, 1), "INV-001", "123-456-7890", 1, "Web Design", "Paid", 500.00m },
                    { 2, "456 Elm St", "Client B", new DateOnly(2023, 2, 15), "example2@example.com", new DateOnly(2023, 2, 1), "INV-002", "987-654-3210", 1, "SEO Services", "Unpaid", 300.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
