namespace Domain.Book
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
