namespace Domain.User
{
    public class User
    {
        public UserId Id { get; }
        public UserContact Email { get; private set; }
        public UserContact Phone { get; private set; }
            List<UserWish> Wishlist = [];
        public UserRegistrationDate RegistrationDate { get; private set; }
        List<SwapBook.SwapBook> SwapBookList = [];
        public User(UserId id, UserContact email, UserContact phone, UserWish wishlist, UserRegistrationDate registrationDate)
        {
            Id = id;
            Email = email;
            Phone = phone;
            Wishlist = wishlist;
            RegistrationDate = registrationDate;
        }
    }
}
