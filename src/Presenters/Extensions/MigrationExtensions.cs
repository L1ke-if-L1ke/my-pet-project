using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Infrastructure.Persistence; 

namespace Presenters.Extensions; 

public static class MigrationExtensions
{
    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        try
        {
            app.Logger.LogInformation("Applying database migrations...");  // Лог начала

            await using var scope = app.Services.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.MigrateAsync();

            app.Logger.LogInformation("Database migrations applied successfully");  // Лог успеха
        }
        catch (Exception ex)
        {
            // Логируем ошибку с полным стектрейсом
            app.Logger.LogError(ex, "Failed to apply database migrations. Application will not start.");

            // Пробрасываем исключение дальше — приложение упадёт (Fail Fast) ✅
            throw;
        }
    }
}