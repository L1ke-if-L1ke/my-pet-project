using Domain.ProjectContexts;
using Domain.ProjectContexts.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Common;
using Microsoft.Extensions.Options;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    // Поле для хранения настроек подключения к БД
    private readonly DatabaseConnectionOptions _dbOptions;

    // DbSet для доступа к сущности Project
    public DbSet<Project> Projects => Set<Project>();

    // Конструктор принимает стандартные опции и настройки подключения через DI
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IOptions<DatabaseConnectionOptions> dbOptions)
        : base(options)
    {
        _dbOptions = dbOptions.Value;
    }

    // Настройка подключения к БД при отсутствии явной конфигурации
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = _dbOptions.CreateConnectionString();
            optionsBuilder.UseNpgsql(connectionString);
        }
    }

    // Применение конфигураций сущностей из текущей сборки
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}