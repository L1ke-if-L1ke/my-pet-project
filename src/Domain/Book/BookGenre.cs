namespace Domain.Book
{
    public record BookGenre
    {
        public string Title { get; set; }
        public BookGenre(string title)
        {
            Title = title;
        }

    }
}
