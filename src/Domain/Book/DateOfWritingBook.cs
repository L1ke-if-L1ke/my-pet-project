namespace Domain.Book
{
    public record DateOfWritingBook
    {
        public DateOnly Date { get; }

        public DateOfWritingBook(DateOnly date)
        {
            Date = date;
        }
    }
}
