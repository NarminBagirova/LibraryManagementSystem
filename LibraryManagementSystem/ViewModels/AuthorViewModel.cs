using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.ViewModels
{
    public class AuthorViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        [StringLength(100)]
        public string? Surname { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(100)]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Invalid Phone Number")]
        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}

