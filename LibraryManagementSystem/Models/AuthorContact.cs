namespace LibraryManagementSystem.Models
{
    public class AuthorContact
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
