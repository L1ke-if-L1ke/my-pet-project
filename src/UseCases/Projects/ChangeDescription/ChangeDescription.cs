namespace UseCases.Projects.ChangeDescription;

public sealed record ChangeProjectDescriptionCommand(Guid Id, string NewDescription);