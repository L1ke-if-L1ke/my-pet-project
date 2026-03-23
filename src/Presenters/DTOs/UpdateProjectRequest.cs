namespace Presenters.DTOs;

/// <summary>
/// Запрос на полное обновление проекта
/// </summary>
public class UpdateProjectRequest
{
    /// <summary>
    /// Новое название проекта
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Новое описание проекта
    /// </summary>
    public string Description { get; set; } = string.Empty;
}