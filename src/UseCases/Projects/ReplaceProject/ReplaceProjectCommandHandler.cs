using Domain.ProjectContexts;
using Domain.ProjectContexts.Entities;
using YourProject.Domain.Interfaces;

namespace UseCases.Projects.ReplaceProject;

public sealed class ReplaceProjectCommandHandler
{
    private readonly IProjectRepository _repository;
    public ReplaceProjectCommandHandler(IProjectRepository repository) => _repository = repository;

    public async Task<Project> Handle(ReplaceProjectCommand command, CancellationToken ct)
    {
        var existing = await _repository.GetByIdAsync(command.Id, ct)
            ?? throw new KeyNotFoundException($"Project with id {command.Id} not found");

        // Валидация через доменные объекты (обязательные поля)
        var name = ProjectName.Create(command.Name);
        var description = ProjectDescription.Create(command.Description);

        // Полная замена: создаём новый агрегат
        var replaced = new Project(
            id: existing.Id,
            lifeTime: existing.LifeTime, // даты жизненного цикла не меняем при замене данных
            description: description,
            name: name,
            tasks: existing.Tasks // задачи сохраняем
        );

        return replaced;
    }
}