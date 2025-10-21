namespace AspireApp1.ServiceDefaults.CatalogBook
{
    public record BookAmount
    {
        public int Amount { get; }
        public BookAmount(int amount)
        {
            Amount = amount;
        }
    }
}
