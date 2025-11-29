using System.ComponentModel.DataAnnotations;

namespace LoginPractice.Models
{
    public class User
    {
        int id { get; set; }

        [MaxLength(100)]
        [Required]
        public string? Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? PasswordConformation { get; set; }

    }
}
