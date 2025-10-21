namespace AspireApp1.ServiceDefaults.User.User
{
    public record UserContact
    {
        public string UserEmail { get; }
        public string UserPhone { get; }
        public UserContact(string email, string phone)
        {
            UserEmail = email;
            UserPhone = phone;
        }
    }
}
