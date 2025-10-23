namespace Domain.User
{
    public record UserRegistrationDate
    {
        public DateOnly Date { get; }
        public UserRegistrationDate(DateOnly date)
        {
            Date = date;
        }
    }
}
