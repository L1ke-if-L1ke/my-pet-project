namespace Domain.SwapBook
{
    public record UserId
    {
        public Guid userId { get; }
        public UserId(Guid id)
        {
            userId = id;
        }
    }
}
