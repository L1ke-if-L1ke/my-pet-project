using Domain.Interfaces;
using Domain.ProjectContexts;
using UseCases.Interfaces;
using YourProject.Domain.Interfaces;

namespace UseCases.Projects.Tasks.CreateTask;

public sealed class CreateTaskCommandHandler
{
    private readonly IProjectRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITransactionFactory _transactionFactory;

    public CreateTaskCommandHandler(
        IProjectRepository repository,
        IUnitOfWork unitOfWork,
        ITransactionFactory transactionFactory)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _transactionFactory = transactionFactory;
    }

    public async Task Handle(CreateTaskCommand command, CancellationToken ct)
    {
        await using var tx = await _transactionFactory.CreateAsync(ct); // Начало транзакции

        var project = await _repository.GetByIdWithLockAsync(command.ProjectId, ct);
        if (project == null)
            throw new KeyNotFoundException($"Project {command.ProjectId} not found");

        project.AddTask(command.Description, command.MembersLimit);

        await _repository.UpdateAsync(project, ct);
        await _unitOfWork.SaveChangesAsync(ct); // Сохранение через UoW

        await tx.CommitAsync(ct); // Коммит
        // Если ошибка до этого — транзакция откатится сама в Dispose
    }
}