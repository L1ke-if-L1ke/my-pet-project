namespace AspireApp1.ServiceDefaults.Book
{
    public record BookName
    {
        public string Name { get; }
        public BookName(string name)
        {
            Name = name;
        }
    }
}
