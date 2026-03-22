using Domain.ProjectContexts;
using Domain.ProjectContexts.Entities;
using Microsoft.AspNetCore.Mvc;
using Presenters.Common;
using Presenters.DTOs;
using YourProject.Domain.Interfaces;

namespace Presenters.Controllers.ProjectsContext;

/// <summary>
/// Контроллер для управления проектами.
/// Предоставляет CRUD-операции над проектами.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectRepository _repository;
    private readonly ILogger<ProjectsController> _logger;

    /// <summary>
    /// Конструктор контроллера проектов
    /// </summary>
    /// <param name="repository">Репозиторий проектов</param>
    /// <param name="logger">Логгер</param>
    public ProjectsController(IProjectRepository repository, ILogger<ProjectsController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// Возвращает список всех проектов
    /// </summary>
    /// <returns>Список проектов в формате Envelope</returns>
    /// <response code="200">Успешное получение списка проектов</response>
    [HttpGet]
    [ProducesResponseType(typeof(Envelope), 200)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        _logger.LogInformation("Getting all projects");

        var projects = await _repository.GetAllAsync(ct);
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
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        _logger.LogInformation("Getting project by id: {Id}", id);

        var project = await _repository.GetByIdAsync(id, ct);
        if (project is null)
        {
            _logger.LogWarning("Project not found: {Id}", id);
            return NotFound(Envelope.ErrorResponse(404, $"Project with id {id} not found"));
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
    public async Task<IActionResult> Create([FromBody] CreateProjectRequest request, CancellationToken ct)
    {
        _logger.LogInformation("Creating project: {Name}", request.Name);

        // Валидация
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(Envelope.ErrorResponse(400, "Project name is required"));
        }

        if (string.IsNullOrWhiteSpace(request.Description))
        {
            return BadRequest(Envelope.ErrorResponse(400, "Project description is required"));
        }

        try
        {
            // Создаём доменную сущность через конструктор с Value Objects
            var project = new Project(
                id: ProjectId.Create(Guid.NewGuid()),
                lifeTime: ProjectLifeTime.Create(DateOnly.FromDateTime(DateTime.UtcNow), null),
                description: ProjectDescription.Create(request.Description),
                name: ProjectName.Create(request.Name),
                tasks: Array.Empty<ProjectTask>()
            );

            await _repository.AddAsync(project, ct);
            _logger.LogInformation("Project created with id: {Id}", project.Id.Value);

            return CreatedAtAction(
                nameof(GetById),
                new { id = project.Id.Value },
                Envelope.Ok(ProjectDto.FromEntity(project))
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(Envelope.ErrorResponse(400, ex.Message));
        }
    }

    /// <summary>
    /// Полностью обновляет существующий проект
    /// </summary>
    /// <param name="id">GUID проекта</param>
    /// <param name="request">Новые данные проекта</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Обновлённый проект</returns>
    /// <response code="200">Проект успешно обновлён</response>
    /// <response code="404">Проект не найден</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Envelope), 200)]
    [ProducesResponseType(typeof(Envelope), 404)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProjectRequest request, CancellationToken ct)
    {
        _logger.LogInformation("Updating project: {Id}", id);

        var existing = await _repository.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound(Envelope.ErrorResponse(404, $"Project with id {id} not found"));
        }

        try
        {
            // Создаём новый объект Project (т.к. свойства read-only)
            var updated = new Project(
                id: existing.Id, // сохраняем тот же ID
                lifeTime: existing.LifeTime, // сохраняем даты
                description: ProjectDescription.Create(request.Description),
                name: ProjectName.Create(request.Name),
                tasks: existing.Tasks // сохраняем задачи
            );

            await _repository.UpdateAsync(updated, ct);
            _logger.LogInformation("Project updated: {Id}", id);

            return Ok(Envelope.Ok(ProjectDto.FromEntity(updated)));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(Envelope.ErrorResponse(400, ex.Message));
        }
    }

    /// <summary>
    /// Частично обновляет проект (только указанные поля)
    /// </summary>
    /// <param name="id">GUID проекта</param>
    /// <param name="request">Поля для обновления</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Обновлённый проект</returns>
    /// <response code="200">Проект успешно обновлён</response>
    /// <response code="404">Проект не найден</response>
    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(Envelope), 200)]
    [ProducesResponseType(typeof(Envelope), 404)]
    public async Task<IActionResult> Patch(Guid id, [FromBody] PatchProjectRequest request, CancellationToken ct)
    {
        _logger.LogInformation("Patching project: {Id}", id);

        var existing = await _repository.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound(Envelope.ErrorResponse(404, $"Project with id {id} not found"));
        }

        try
        {
            // Частичное обновление: используем новые значения если переданы, иначе старые
            var newName = string.IsNullOrWhiteSpace(request.Name)
                ? existing.Name.Value
                : request.Name;

            var newDescription = string.IsNullOrWhiteSpace(request.Description)
                ? existing.Description.Value
                : request.Description;

            // Создаём новый объект Project (т.к. свойства read-only)
            var patched = new Project(
                id: existing.Id, // сохраняем ID
                lifeTime: existing.LifeTime, // сохраняем даты создания
                description: ProjectDescription.Create(newDescription),
                name: ProjectName.Create(newName),
                tasks: existing.Tasks // сохраняем задачи
            );

            await _repository.UpdateAsync(patched, ct);
            _logger.LogInformation("Project patched: {Id}", id);

            return Ok(Envelope.Ok(ProjectDto.FromEntity(patched)));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(Envelope.ErrorResponse(400, ex.Message));
        }
    }
    /// 
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Envelope), 200)]
    [ProducesResponseType(typeof(Envelope), 404)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        _logger.LogInformation("Deleting project: {Id}", id);

        var deleted = await _repository.DeleteAsync(id, ct);
        if (!deleted)
        {
            return NotFound(Envelope.ErrorResponse(404, $"Project with id {id} not found"));
        }

        _logger.LogInformation("Project deleted: {Id}", id);
        return Ok(Envelope.Ok(new { message = "Project deleted successfully" }));
    }
}