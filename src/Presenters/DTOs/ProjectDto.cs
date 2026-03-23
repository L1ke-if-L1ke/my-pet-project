using Domain.ProjectContexts;
using Domain.ProjectContexts.Entities;

namespace Presenters.DTOs;

/// <summary>
/// DTO для передачи данных о проекте клиенту.
/// Не содержит доменной логики — только данные.
/// </summary>
public class ProjectDto
{
    /// <summary>
    /// Уникальный идентификатор проекта
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Название проекта
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Описание проекта
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Дата создания проекта
    /// </summary>
    public DateOnly CreatedAt { get; set; }

    /// <summary>
    /// Маппинг из доменной сущности в DTO
    /// </summary>
    public static ProjectDto FromEntity(Project project)
    {
        return new ProjectDto
        {
            Id = project.Id.Value,
            Name = project.Name.Value,
            Description = project.Description.Value,
            CreatedAt = project.LifeTime.CreatedAt
        };
    }
}