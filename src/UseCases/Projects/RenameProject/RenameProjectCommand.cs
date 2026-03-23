namespace UseCases.Projects.RenameProject;

public sealed record RenameProjectCommand(Guid Id, string NewName);