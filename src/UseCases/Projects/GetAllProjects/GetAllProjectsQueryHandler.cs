using Domain.ProjectContexts;
using YourProject.Domain.Interfaces;

namespace UseCases.Projects.GetAllProjects;

public sealed class GetAllProjectsQueryHandler
{
    private readonly IProjectRepository _repository;

    public GetAllProjectsQueryHandler(IProjectRepository repository) =>
        _repository = repository;

    public async Task<IReadOnlyList<Project>> Handle(GetAllProjectsQuery query, CancellationToken ct) =>
        await _repository.GetAllAsync(ct);
}