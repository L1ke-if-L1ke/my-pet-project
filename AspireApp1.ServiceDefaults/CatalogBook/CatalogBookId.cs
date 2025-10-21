
namespace AspireApp1.ServiceDefaults.CatalogBook
{
    public record CatalogBookId
    {
        public CatalogBookId id { get; }
        public CatalogBookId(CatalogBookId Id)
        {
            Id = id;
        }
    }
}
