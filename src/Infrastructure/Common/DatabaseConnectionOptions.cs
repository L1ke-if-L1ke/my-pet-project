using Npgsql;

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
            Validate();

            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = HostName,
                Database = DatabaseName,
                Username = UserName,
                Password = Password,
            };

            return builder.ConnectionString;
        }
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(HostName))
                throw new InvalidOperationException("HostName is required");
            if (string.IsNullOrWhiteSpace(Password))
                throw new InvalidOperationException("Password is required");
            if (string.IsNullOrWhiteSpace(UserName))
                throw new InvalidOperationException("UserName is required");
            if (string.IsNullOrWhiteSpace(DatabaseName))
                throw new InvalidOperationException("DatabaseName is required");
        }

    }
}
