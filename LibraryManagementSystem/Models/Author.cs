namespace LibraryManagementSystem.Models
{
    public class Author:BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FullName => $"{Name} {Surname}";
        public AuthorContact? AuthorContact { get; set; }
        public ICollection<BookAuthor> BookAuthors { get; set; }

    }
}
