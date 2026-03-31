using Microsoft.Extensions.DependencyInjection;  // Для IServiceCollection
using Microsoft.AspNetCore.Builder;              // Для WebApplication (на всякий случай)
using Microsoft.OpenApi.Models;
using System.Reflection;
using Infrastructure;
using UseCases;
using Presenters.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Project API",
        Version = "v1"
    });
    // Подключаем XML-комментарии из кода
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.ApplyMigrationsAsync(); // Применение миграций

app.Run();