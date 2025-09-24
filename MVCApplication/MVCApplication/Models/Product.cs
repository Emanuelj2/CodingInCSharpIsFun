using System.ComponentModel.DataAnnotations;

namespace MVCApplication.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Required]
        [Range(0.01, 10000.00)]
        public decimal Price { get; set; }
    }
}
