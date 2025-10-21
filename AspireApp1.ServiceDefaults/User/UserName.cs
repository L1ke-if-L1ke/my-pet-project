
namespace AspireApp1.ServiceDefaults.User.User
{
    public record UserName
    {
        public string Name { get; set; }
        public UserName(string name)
        {
            Name = name;
        }
}
