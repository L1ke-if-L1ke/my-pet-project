using System.Net;
using Domain.ProjectContexts;
using Domain.ProjectContexts.Entities;
using Microsoft.AspNetCore.Mvc;
using Presenters.Common;
using Presenters.DTOs;
using YourProject.Domain.Interfaces;
using UseCases.Projects.Tasks.CreateTask;
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
/// Предоставляет CRUD-операции над проектами через Application-слой (UseCases).
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly ILogger<ProjectsController> _logger;

    /// <summary>
    /// Конструктор контроллера проектов
    /// </summary>
    /// <param name="logger">Логгер для записи событий</param>
    public ProjectsController(ILogger<ProjectsController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Возвращает список всех проектов
    /// </summary>
    /// <param name="handler">Обработчик запроса GetAllProjectsQuery</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Список проектов в формате Envelope</returns>
    /// <response code="200">Успешное получение списка проектов</response>
    [HttpGet]
    [ProducesResponseType(typeof(Envelope), 200)]
    public async Task<IActionResult> GetAll(
        [FromServices] GetAllProjectsQueryHandler handler,
        CancellationToken ct)
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
    /// <param name="handler">Обработчик запроса GetProjectByIdQuery</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Проект или ошибка 404</returns>
    /// <response code="200">Проект найден</response>
    /// <response code="404">Проект не найден</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Envelope), 200)]
    [ProducesResponseType(typeof(Envelope), 404)]
    public async Task<IActionResult> GetById(
        Guid id,
        [FromServices] GetProjectByIdQueryHandler handler,
        CancellationToken ct)
    {
        _logger.LogInformation("Getting project by id: {Id}", id);

        var project = await handler.Handle(new GetProjectByIdQuery(id), ct);
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
    /// <param name="handler">Обработчик команды CreateProjectCommand</param>
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
    /// <param name="id">GUID проекта для замены</param>
    /// <param name="request">Новые данные проекта (имя и описание)</param>
    /// <param name="handler">Обработчик команды ReplaceProjectCommand</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Обновлённый проект</returns>
    /// <response code="200">Проект успешно заменён</response>
    /// <response code="400">Некорректные данные запроса</response>
    /// <response code="404">Проект не найден</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Envelope), 200)]
    [ProducesResponseType(typeof(Envelope), 400)]
    [ProducesResponseType(typeof(Envelope), 404)]
    public async Task<IActionResult> Replace(
        Guid id,
        [FromBody] ReplaceProjectRequest request,
        [FromServices] ReplaceProjectCommandHandler handler,
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
    /// Переименовать проект (операционное обновление)
    /// </summary>
    /// <param name="id">GUID проекта для переименования</param>
    /// <param name="request">Запрос с новым именем проекта</param>
    /// <param name="handler">Обработчик команды RenameProjectCommand</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Обновлённый проект</returns>
    /// <response code="200">Проект успешно переименован</response>
    /// <response code="400">Некорректное имя проекта</response>
    /// <response code="404">Проект не найден</response>
    [HttpPatch("{id}/rename")]
    [ProducesResponseType(typeof(Envelope), 200)]
    [ProducesResponseType(typeof(Envelope), 400)]
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
    /// Изменить описание проекта (операционное обновление)
    /// </summary>
    /// <param name="id">GUID проекта</param>
    /// <param name="request">Запрос с новым описанием</param>
    /// <param name="handler">Обработчик команды ChangeProjectDescriptionCommand</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Обновлённый проект</returns>
    /// <response code="200">Описание успешно изменено</response>
    /// <response code="400">Некорректное описание</response>
    /// <response code="404">Проект не найден</response>
    [HttpPatch("{id}/change-description")]
    [ProducesResponseType(typeof(Envelope), 200)]
    [ProducesResponseType(typeof(Envelope), 400)]
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

    /// <summary>
    /// Удаляет проект по уникальному идентификатору
    /// </summary>
    /// <param name="id">GUID проекта для удаления</param>
    /// <param name="handler">Обработчик команды DeleteProjectCommand</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Результат операции удаления</returns>
    /// <response code="200">Проект успешно удалён</response>
    /// <response code="404">Проект не найден</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Envelope), 200)]
    [ProducesResponseType(typeof(Envelope), 404)]
    public async Task<IActionResult> Delete(
        Guid id,
        [FromServices] DeleteProjectCommandHandler handler,
        CancellationToken ct)
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