using System.ComponentModel.DataAnnotations;

namespace Presenters.DTOs;
public sealed record CreateTaskRequest(
    [Required][StringLength(500, MinimumLength = 1)] string Description,
    [Range(1, 100, ErrorMessage = "Members limit must be between 1 and 100")] int MembersLimit
);