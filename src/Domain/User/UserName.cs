using Domain.Book;

namespace Domain.User
{
    public record UserName
    {
        public string Name { get; set; }
        private UserName(string name)
        {
            Name = name;
        }
        public static UserName Create(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("Имя было пустым");
            string formatted = value.Trim();
            return new UserName(formatted);
        }
    }
}