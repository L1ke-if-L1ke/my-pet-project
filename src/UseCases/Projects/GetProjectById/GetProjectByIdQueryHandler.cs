using Domain.ProjectContexts;
using YourProject.Domain.Interfaces;

namespace UseCases.Projects.GetProjectById;

public sealed class GetProjectByIdQueryHandler
{
    private readonly IProjectRepository _repository;

    public GetProjectByIdQueryHandler(IProjectRepository repository) =>
        _repository = repository;

    public async Task<Project?> Handle(GetProjectByIdQuery query, CancellationToken ct) =>
        await _repository.GetByIdAsync(query.Id, ct);
}