namespace Domain.Book
{
    public class Book
    {
        public BookId Id { get; }
        public BookGenre Title { get; private set; }

        public BookName Name { get; private set}
        public DateOfWritingBook DateOfWritingBook { get; private set; }
        public SwapBook.SwapBook? SwapBook { get; }
        public Book(BookId id, BookGenre title, BookName name, DateOfWritingBook dateOfWritingBook)
        {
            Id = id;
            Title = title;
            Name = name;
            DateOfWritingBook = dateOfWritingBook;
            SwapBook = null;
        }
    }
}
