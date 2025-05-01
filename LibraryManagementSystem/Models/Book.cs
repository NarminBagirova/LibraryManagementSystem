namespace LibraryManagementSystem.Models
{
    public class Book:BaseEntity
    {
        public string Title {  get; set; }
        public int BookCategoryId {  get; set; }
        public BookCategory BookCategory {  get; set; }
        public int PublisherId {  get; set; }
        public Publisher Publisher { get; set; }
        public ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
