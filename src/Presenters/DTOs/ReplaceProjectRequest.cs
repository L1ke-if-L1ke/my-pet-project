namespace Presenters.DTOs;
public class ReplaceProjectRequest
{
    public string Name { get; set; } = string.Empty;      // обязательное
    public string Description { get; set; } = string.Empty; // обязательное
}