using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.ViewModels
{
    public class BookViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title can't be longer than 200 characters")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Category selection is required")]
        public int BookCategoryId { get; set; }

        [Required(ErrorMessage = "Publisher selection is required")]
        public int PublisherId { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [Required(ErrorMessage = "At least one author must be selected")]
        public List<int> SelectedAuthorIds { get; set; } = new List<int>();

        public List<SelectListItem> Authors { get; set; } = new List<SelectListItem>();
        public List<SelectListItem>? BookCategories { get; set; }
        public List<SelectListItem>? Publishers { get; set; }
    }
}
