namespace Domain.User
{
    public record UserName
    {
        public string Name { get; set; }
        public UserName(string name)
        {
            Name = name;
        }
    }
