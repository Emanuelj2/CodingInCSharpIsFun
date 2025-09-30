using Microsoft.EntityFrameworkCore;
using MVCApplication.Data;
using MVCApplication.Models;

namespace MVCApplication.Services
{
    public interface IInvoiceService
    {
        // This interface is intended to define methods related to invoice operations.
        // Currently, it does not contain any methods.

        //get all invoices
        Task<List<Invoice>> GetAllInvoicesAsync();
        //get invoice by id
        Task<Invoice> GetInvoiceByIdAsync(int invoiceId);

        //create invoice
        Task<Invoice> CreateInvoiceAsync(Invoice invoice);
        //edit invoice
        Task<Invoice> UpdateInvoiceAsync(Invoice invoice);
        //delete invoice
        Task<bool> DeleteInvoiceAsync(int invoiceId);

    }
    public class InvoiceService : IInvoiceService
    {
        // This class is intended to handle business logic related to invoices.
        // Currently, it does not contain any methods or properties.
        public readonly AppDbContext _db;
        public InvoiceService(AppDbContext db) 
        { 
            _db = db;
        }

        public async Task<List<Invoice>> GetAllInvoicesAsync()
        {
            return await _db.Invoices.ToListAsync();
        }

        public async Task<Invoice> GetInvoiceByIdAsync(int invoiceId)
        {
            var invoice = await _db.Invoices.FindAsync(invoiceId);

            if (invoice == null) 
            {
                throw new KeyNotFoundException("Invoice not found");
            }
            return invoice;
        }

        public async Task<Invoice> CreateInvoiceAsync(Invoice invoice)
        {
            _db.Invoices.Add(invoice);
            await _db.SaveChangesAsync();
            return invoice;
        }

        public async Task<Invoice> UpdateInvoiceAsync(Invoice invoice)
        {
            var existingInvoice = await _db.Invoices.FindAsync(invoice.Id);
            if (existingInvoice != null)
            {
                existingInvoice.Number = invoice.Number;
                existingInvoice.Status = invoice.Status;
                existingInvoice.IssueDate = invoice.IssueDate;
                existingInvoice.DueDate = invoice.DueDate;
                existingInvoice.Service = invoice.Service;
                existingInvoice.UnitPrice = invoice.UnitPrice;
                existingInvoice.Quantity = invoice.Quantity;
                existingInvoice.ClientName = invoice.ClientName;
                existingInvoice.Email = invoice.Email;
                existingInvoice.Phone = invoice.Phone;
                existingInvoice.Address = invoice.Address;
                await _db.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Invoice not found");
            }
            return existingInvoice;
        }

        public async Task<bool> DeleteInvoiceAsync(int invoiceId)
        {
            var invoice = await _db.Invoices.FindAsync(invoiceId);
            if (invoice != null)
            {
                _db.Invoices.Remove(invoice);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
