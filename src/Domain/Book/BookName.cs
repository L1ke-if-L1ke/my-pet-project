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
            return new BookName(formatted);
        }
    }
}
