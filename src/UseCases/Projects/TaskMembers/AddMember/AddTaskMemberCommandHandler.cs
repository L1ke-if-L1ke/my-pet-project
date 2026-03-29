using Domain.Interfaces;
using UseCases.Interfaces;
using YourProject.Domain.Interfaces;

namespace UseCases.Projects.TaskMembers.AddMember;

public sealed class AddTaskMemberCommandHandler
{
    private readonly IProjectRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITransactionFactory _transactionFactory;

    public AddTaskMemberCommandHandler(
        IProjectRepository repository,
        IUnitOfWork unitOfWork,
        ITransactionFactory transactionFactory)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _transactionFactory = transactionFactory;
    }

    public async Task Handle(AddTaskMemberCommand command, CancellationToken ct)
    {
        // Начинаем транзакцию
        await using var tx = await _transactionFactory.CreateAsync(ct);

        // Загружаем агрегат (с задачами и участниками!)
        var project = await _repository.GetByIdWithLockAsync(command.ProjectId, ct);
        if (project == null)
            throw new KeyNotFoundException($"Project {command.ProjectId} not found");

        //  Выполняем доменную логику
        project.AddMemberToTask(command.TaskId, command.Email, command.Login);

        // КРИТИЧНО: явно сообщаем контексту, что агрегат изменён
        // (для вложенных owned-коллекций это обязательно)
        await _repository.UpdateAsync(project, ct);

        // Сохраняем и коммитим
        await _unitOfWork.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
    }
}