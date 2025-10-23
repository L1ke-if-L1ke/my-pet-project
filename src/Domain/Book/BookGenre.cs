namespace Domain.Book
{
    public record BookGenre
    {
        public string Title { get; set; }
        public BookGenre(string title)
        {
            Title = title;
        }
        public static BookGenre Create(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("Название жанра было пустым");
            string formatted = value.Trim();
            return new BookGenre(formatted);
        }
    }
}
