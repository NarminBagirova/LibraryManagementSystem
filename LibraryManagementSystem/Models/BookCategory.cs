namespace LibraryManagementSystem.Models
{
    public class BookCategory:BaseEntity
    {
        public string Name {  get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
