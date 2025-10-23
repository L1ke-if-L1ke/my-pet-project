using Domain.Book;

namespace Domain.User
{
    public record UserWish
    {
        public string Value;
        public UserId UserId;
        public BookId BookId;

        public UserWish(string value)
        {
            Value = value;
        }
    }
}
