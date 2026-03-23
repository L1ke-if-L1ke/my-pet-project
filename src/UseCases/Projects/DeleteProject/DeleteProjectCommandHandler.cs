using YourProject.Domain.Interfaces;

namespace UseCases.Projects.DeleteProject;

public sealed class DeleteProjectCommandHandler
{
    private readonly IProjectRepository _repository;

    public DeleteProjectCommandHandler(IProjectRepository repository) =>
        _repository = repository;

    /// <returns>True если проект был удалён, False если не найден</returns>
    public async Task<bool> Handle(DeleteProjectCommand command, CancellationToken ct) =>
        await _repository.DeleteAsync(command.Id, ct);
}