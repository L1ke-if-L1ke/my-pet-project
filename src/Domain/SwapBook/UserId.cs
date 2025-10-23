namespace Domain.SwapBook
{
    public record UserId
    {
        public Guid UserId { get; }
        public UserId(Guid id)
        {
            UserId = id;
        }
    }
}
