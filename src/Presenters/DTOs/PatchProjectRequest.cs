namespace Presenters.DTOs;

/// <summary>
/// Запрос на частичное обновление проекта
/// </summary>
public class PatchProjectRequest
{
    /// <summary>
    /// Новое название (опционально)
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Новое описание (опционально)
    /// </summary>
    public string? Description { get; set; }
}