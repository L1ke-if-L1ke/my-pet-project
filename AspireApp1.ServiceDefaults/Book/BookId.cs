namespace AspireApp1.ServiceDefaults.Book
{
    public record BookId
    {
        public BookId Id { get; }

        public BookId(BookId id)
        {
            Id = id;
        }
    }
}
