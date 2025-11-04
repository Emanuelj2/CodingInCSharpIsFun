using Microsoft.EntityFrameworkCore;

namespace MVCApplication.Models
{
    public class Invoice
    {
        public int Id { get; set; }


        public string Number { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateOnly IssueDate { get; set; }
        public DateOnly DueDate { get; set; }


        //service details
        public string Service { get; set; } = string.Empty;

        [Precision(18, 2)]
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }


        //client details
        public string ClientName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

    }
}
