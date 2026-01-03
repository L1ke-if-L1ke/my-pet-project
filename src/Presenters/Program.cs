using Infrastructure.Common;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

DatabaseConnectionOptions? connectionOptions = builder
    .Configuration.GetSection(nameof(DatabaseConnectionOptions))
    .Get<DatabaseConnectionOptions>();

if (connectionOptions == null)
    throw new ArgumentException();

Console.WriteLine(connectionOptions.DatabaseName);
Console.WriteLine(connectionOptions.UserName);
Console.WriteLine(connectionOptions.Password);
Console.WriteLine(connectionOptions.HostName);

WebApplication app = builder.Build();