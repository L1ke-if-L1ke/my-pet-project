namespace AspireApp1.ServiceDefaults.CatalogBook
{
    public record CatalogBook
    {
        public CatalogBookId Id { get; }
        public BookAmount Amount { get; private set; }
        public List<Book.Book> Books = [];
        public CatalogBook(CatalogBookId id, BookAmount amount)
        {
            Id = id;
            Amount = amount;
        }
    }
}
