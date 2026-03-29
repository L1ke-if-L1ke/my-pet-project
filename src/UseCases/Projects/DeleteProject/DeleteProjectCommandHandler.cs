using Domain.Interfaces;
using Domain.ProjectContexts;
using UseCases.Interfaces;
using YourProject.Domain.Interfaces;

namespace UseCases.Projects.DeleteProject;

public sealed class DeleteProjectCommandHandler
{

    private readonly IProjectRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITransactionFactory _transactionFactory;

    public DeleteProjectCommandHandler(
        IProjectRepository repository,
        IUnitOfWork unitOfWork,
        ITransactionFactory transactionFactory)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _transactionFactory = transactionFactory;
    }
    public async Task<bool> Handle(DeleteProjectCommand command, CancellationToken ct)
    {
        await using var tx = await _transactionFactory.CreateAsync(ct);

        var project = await _repository.GetByIdAsync(command.Id, ct);
        if (project == null)
            return false;

        await _repository.DeleteAsync(project, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        await tx.CommitAsync(ct);

        return true;
    }
}