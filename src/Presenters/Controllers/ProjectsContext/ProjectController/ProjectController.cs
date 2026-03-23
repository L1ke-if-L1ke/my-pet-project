using System.Net;
using Domain.ProjectContexts;
using Domain.ProjectContexts.Entities;
using Microsoft.AspNetCore.Mvc;
using Presenters.Common;
using Presenters.DTOs;
using YourProject.Domain.Interfaces;
using UseCases.Projects.CreateProject;
using UseCases.Projects.GetAllProjects;
using UseCases.Projects.GetProjectById;
using UseCases.Projects.DeleteProject;
using UseCases.Projects.ReplaceProject;
using UseCases.Projects.RenameProject;
using UseCases.Projects.ChangeDescription;

namespace Presenters.Controllers.ProjectsContext;

/// <summary>
/// Контроллер для управления проектами.
/// Предоставляет CRUD-операции над проектами.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly ILogger<ProjectsController> _logger;

    /// <summary>
    /// Конструктор контроллера проектов
    /// </summary>
    /// <param name="repository">Репозиторий проектов</param>
    /// <param name="logger">Логгер</param>
    public ProjectsController(ILogger<ProjectsController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Возвращает список всех проектов
    /// </summary>
    /// <returns>Список проектов в формате Envelope</returns>
    /// <response code="200">Успешное получение списка проектов</response>
    [HttpGet]
    [ProducesResponseType(typeof(Envelope), 200)]
    public async Task<IActionResult> GetAll([FromServices] GetAllProjectsQueryHandler handler, CancellationToken ct)
    {
        _logger.LogInformation("Getting all projects");

        var projects = await handler.Handle(new GetAllProjectsQuery(), ct);
        var dtos = projects.Select(ProjectDto.FromEntity).ToList();

        return Ok(Envelope.Ok(dtos));
    }

    /// <summary>
    /// Возвращает проект по уникальному идентификатору
    /// </summary>
    /// <param name="id">GUID проекта</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Проект или ошибка 404</returns>
    /// <response code="200">Проект найден</response>
    /// <response code="404">Проект не найден</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Envelope), 200)]
    [ProducesResponseType(typeof(Envelope), 404)]
    public async Task<IActionResult> GetById(Guid id, [FromServices] GetProjectByIdQueryHandler handler, CancellationToken ct)
    {
        _logger.LogInformation("Getting project by id: {Id}", id);

        var project = await handler.Handle(new GetProjectByIdQuery(id), ct);  // ← ЧЕРЕЗ ХЭНДЛЕР
        if (project is null)
        {
            _logger.LogWarning("Project not found: {Id}", id);
            return NotFound(Envelope.ErrorResponse(HttpStatusCode.NotFound, $"Project with id {id} not found"));
        }

        return Ok(Envelope.Ok(ProjectDto.FromEntity(project)));
    }

    /// <summary>
    /// Создаёт новый проект
    /// </summary>
    /// <param name="request">Данные для создания проекта</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Созданный проект</returns>
    /// <response code="201">Проект успешно создан</response>
    /// <response code="400">Некорректные данные запроса</response>
    [HttpPost]
    [ProducesResponseType(typeof(Envelope), 201)]
    [ProducesResponseType(typeof(Envelope), 400)]
    public async Task<IActionResult> Create(
        [FromBody] CreateProjectRequest request,
        [FromServices] CreateProjectCommandHandler handler,
        CancellationToken ct)
    {
        _logger.LogInformation("Creating project: {Name}", request.Name);

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(Envelope.ErrorResponse(HttpStatusCode.BadRequest, "Project name is required"));
        }

        if (string.IsNullOrWhiteSpace(request.Description))
        {
            return BadRequest(Envelope.ErrorResponse(HttpStatusCode.BadRequest, "Project description is required"));
        }

        try
        {
            var command = new CreateProjectCommand(request.Name, request.Description);

            var project = await handler.Handle(command, ct);

            _logger.LogInformation("Project created with id: {Id}", project.Id.Value);

            return CreatedAtAction(
                nameof(GetById),
                new { id = project.Id.Value },
                Envelope.Ok(ProjectDto.FromEntity(project))
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(Envelope.ErrorResponse(HttpStatusCode.BadRequest, ex.Message));
        }
    }


    /// <summary>
    /// Полная замена проекта (все обязательные поля)
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Envelope), 200)]
    [ProducesResponseType(typeof(Envelope), 404)]
    public async Task<IActionResult> Replace(  // ← переименовали метод
        Guid id,
        [FromBody] ReplaceProjectRequest request,  // ← новый DTO
        [FromServices] ReplaceProjectCommandHandler handler,  // ← хэндлер через DI
        CancellationToken ct)
    {
        _logger.LogInformation("Replacing project: {Id}", id);

        try
        {
            var command = new ReplaceProjectCommand(id, request.Name, request.Description);
            var project = await handler.Handle(command, ct);
            return Ok(Envelope.Ok(ProjectDto.FromEntity(project)));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(Envelope.ErrorResponse(HttpStatusCode.NotFound, ex.Message));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(Envelope.ErrorResponse(HttpStatusCode.BadRequest, ex.Message));
        }
    }

    /// <summary>
    /// Переименовать проект
    /// </summary>
    [HttpPatch("{id}/rename")]
    [ProducesResponseType(typeof(Envelope), 200)]
    [ProducesResponseType(typeof(Envelope), 404)]
    public async Task<IActionResult> Rename(
        Guid id,
        [FromBody] RenameProjectRequest request,
        [FromServices] RenameProjectCommandHandler handler,
        CancellationToken ct)
    {
        _logger.LogInformation("Renaming project {Id} to {NewName}", id, request.NewName);

        try
        {
            var command = new RenameProjectCommand(id, request.NewName);
            var project = await handler.Handle(command, ct);
            return Ok(Envelope.Ok(ProjectDto.FromEntity(project)));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(Envelope.ErrorResponse(HttpStatusCode.NotFound, ex.Message));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(Envelope.ErrorResponse(HttpStatusCode.BadRequest, ex.Message));
        }
    }

    /// <summary>
    /// Изменить описание проекта
    /// </summary>
    [HttpPatch("{id}/change-description")]
    [ProducesResponseType(typeof(Envelope), 200)]
    [ProducesResponseType(typeof(Envelope), 404)]
    public async Task<IActionResult> ChangeDescription(
        Guid id,
        [FromBody] ChangeDescriptionRequest request,
        [FromServices] ChangeProjectDescriptionCommandHandler handler,
        CancellationToken ct)
    {
        _logger.LogInformation("Changing description for project {Id}", id);

        try
        {
            var command = new ChangeProjectDescriptionCommand(id, request.NewDescription);
            var project = await handler.Handle(command, ct);
            return Ok(Envelope.Ok(ProjectDto.FromEntity(project)));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(Envelope.ErrorResponse(HttpStatusCode.NotFound, ex.Message));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(Envelope.ErrorResponse(HttpStatusCode.BadRequest, ex.Message));
        }
    }

    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Envelope), 200)]
    [ProducesResponseType(typeof(Envelope), 404)]
    public async Task<IActionResult> Delete(Guid id, [FromServices] DeleteProjectCommandHandler handler, CancellationToken ct)
    {
        _logger.LogInformation("Deleting project: {Id}", id);

        var deleted = await handler.Handle(new DeleteProjectCommand(id), ct); 
        if (!deleted)
        {
            return NotFound(Envelope.ErrorResponse(HttpStatusCode.NotFound, $"Project with id {id} not found"));
        }

        _logger.LogInformation("Project deleted: {Id}", id);
        return Ok(Envelope.Ok(new { message = "Project deleted successfully" }));
    }
}