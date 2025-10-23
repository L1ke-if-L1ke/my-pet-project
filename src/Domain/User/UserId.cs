namespace Domain.User
{
    public record UserId
    {
        public Guid Id { get; }
        public UserId(Guid id)
        {
            Id = id;
        }
    }
}
