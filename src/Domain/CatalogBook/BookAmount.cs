namespace Domain.CatalogBook
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
