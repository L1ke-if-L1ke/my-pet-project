namespace Presenters.DTOs;

/// <summary>
/// Запрос на создание нового проекта
/// </summary>
public class CreateProjectRequest
{
    /// <summary>
    /// Название проекта (обязательно)
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Описание проекта (обязательно)
    /// </summary>
    public string Description { get; set; } = string.Empty;
}