namespace UseCases.Projects.CreateProject;

public sealed record CreateProjectCommand(
    string Name,
    string Description
);