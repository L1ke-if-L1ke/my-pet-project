using Domain.Interfaces;
using Domain.ProjectContexts;
using UseCases.Interfaces;
using YourProject.Domain.Interfaces;

namespace UseCases.Projects.DeleteProject;

public sealed class DeleteProjectCommandHandler
{

    private readonly IProjectRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    public DeleteProjectCommandHandler(
        IProjectRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task<bool> Handle(DeleteProjectCommand command, CancellationToken ct)
    {

        var project = await _repository.GetByIdAsync(command.Id, ct);
        if (project == null)
            return false;

        await _repository.DeleteAsync(project, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return true;
    }
}