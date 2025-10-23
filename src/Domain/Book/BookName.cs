namespace Domain.Book
{
    public record BookName
    {
        public string Name { get; }
        public BookName(string name)
        {
            Name = name;
        }
        public static BookName Create(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("Имя книги было пустым");
            string formatted = value.Trim();
            if (formatted.Length <= 1)
                throw new ArgumentException("Название книги должно иметь минимум 2 символа");
            return new BookName(formatted);
        }
    }
}
