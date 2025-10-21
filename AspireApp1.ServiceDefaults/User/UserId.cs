namespace AspireApp1.ServiceDefaults.User.User
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
