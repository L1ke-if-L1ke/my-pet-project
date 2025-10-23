namespace Domain.SwapBook
{
    public record BookId
    {
        public BookId bookId { get; }

        public BookId(BookId id)
        {
            bookId = id;
        }
    }
}
