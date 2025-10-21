
namespace AspireApp1.ServiceDefaults.User.User
{
    public class User
    {
        public UserId Id { get; }
        public UserContact Email { get; private set; }
        public UserContact Phone { get; private set; }
        public Wishlist Wishlist { get; }
        public UserRegistrationDate RegistrationDate { get; private set; }
        List<SwapBook.SwapBook> SwapBookList = [];
        public User(UserId id, UserContact email, UserContact phone, Wishlist wishlist, UserRegistrationDate registrationDate)
        {
            Id = id;
            Email = email;
            Phone = phone;
            Wishlist = wishlist;
            RegistrationDate = registrationDate;
        }
    }
}
