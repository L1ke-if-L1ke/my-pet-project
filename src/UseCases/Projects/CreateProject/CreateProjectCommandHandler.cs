using Domain.Interfaces;
using Domain.ProjectContexts;
using Domain.ProjectContexts.Entities;
using UseCases.Interfaces;
using YourProject.Domain.Interfaces;

namespace UseCases.Projects.CreateProject;

public sealed class CreateProjectCommandHandler
{
    private readonly IProjectRepository _repository;
    private readonly IUnitOfWork _unitOfWork;              
    public CreateProjectCommandHandler(
        IProjectRepository repository,
        IUnitOfWork unitOfWork)           
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Project> Handle(CreateProjectCommand command, CancellationToken ct)
    {
        // Валидация домена
        var name = ProjectName.Create(command.Name);
        var description = ProjectDescription.Create(command.Description);

        var project = new Project(
            id: ProjectId.Create(Guid.NewGuid()),
            lifeTime: ProjectLifeTime.Create(DateOnly.FromDateTime(DateTime.UtcNow), null),
            description: description,
            name: name,
            tasks: Array.Empty<ProjectTask>()
        );

        await _repository.AddAsync(project, ct);

        // Сохраняем через Unit of Work
        await _unitOfWork.SaveChangesAsync(ct);

        return project;
    }
}