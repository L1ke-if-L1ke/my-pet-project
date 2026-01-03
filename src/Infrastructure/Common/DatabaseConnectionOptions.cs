namespace Infrastructure.Common
{
    public sealed class DatabaseConnectionOptions
    {
        public required string HostName { get; set; }
        public required string Password { get; set; }
        public required string UserName { get; set; }
        public required string DatabaseName { get; set; }
        public string CreateConnectionString()
        {
            throw new NotImplementedException();
        }

    }
}
