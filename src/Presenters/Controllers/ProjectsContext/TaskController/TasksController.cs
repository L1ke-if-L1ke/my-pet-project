using Microsoft.AspNetCore.Mvc;
using Presenters.Common;
using UseCases.Projects.Tasks.CreateTask;
using UseCases.Projects.Tasks.DeleteTask;
using UseCases.Projects.Tasks.ChangeTaskInfo;
using UseCases.Projects.TaskMembers.AddMember;
using UseCases.Projects.TaskMembers.RemoveMember;
using Presenters.DTOs;

namespace Presenters.Controllers.ProjectsContext;

/// <summary>
/// Контроллер для управления задачами проектов.
/// Предоставляет операции CRUD для задач и управления участниками задач.
/// </summary>
[ApiController]
[Route("api/projects/{projectId}/tasks")]
public class TasksController : ControllerBase
{
    // -------------------------
    // CREATE TASK
    // -------------------------
    /// <summary>
    /// Создаёт новую задачу в проекте
    /// </summary>
    /// <param name="projectId">Идентификатор проекта</param>
    /// <param name="request">Данные для создания задачи</param>
    /// <param name="handler">Обработчик команды CreateTaskCommand</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Результат создания задачи</returns>
    /// <response code="200">Задача успешно создана</response>
    /// <response code="400">Некорректные данные запроса</response>
    /// <response code="404">Проект не найден</response>
    [HttpPost]
    [ProducesResponseType(typeof(Envelope), 200)]
    [ProducesResponseType(typeof(Envelope), 400)]
    [ProducesResponseType(typeof(Envelope), 404)]
    public async Task<IActionResult> CreateTask(
        Guid projectId,
        [FromBody] CreateTaskRequest request,
        [FromServices] CreateTaskCommandHandler handler,
        CancellationToken ct)
    {
        var command = new CreateTaskCommand(
            projectId,
            request.Description,
            request.MembersLimit
        );

        await handler.Handle(command, ct);
        return Ok(Envelope.Ok("Task created"));
    }

    // -------------------------
    // DELETE TASK
    // -------------------------
    /// <summary>
    /// Удаляет задачу из проекта
    /// </summary>
    /// <param name="projectId">Идентификатор проекта</param>
    /// <param name="taskId">Идентификатор задачи</param>
    /// <param name="handler">Обработчик команды DeleteTaskCommand</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Результат удаления</returns>
    /// <response code="200">Задача успешно удалена</response>
    /// <response code="404">Задача или проект не найдены</response>
    [HttpDelete("{taskId}")]
    [ProducesResponseType(typeof(Envelope), 200)]
    [ProducesResponseType(typeof(Envelope), 404)]
    public async Task<IActionResult> DeleteTask(
        Guid projectId,
        Guid taskId,
        [FromServices] DeleteTaskCommandHandler handler,
        CancellationToken ct)
    {
        await handler.Handle(new DeleteTaskCommand(projectId, taskId), ct);
        return Ok(Envelope.Ok("Task deleted"));
    }

    // -------------------------
    // CHANGE TASK INFO
    // -------------------------
    /// <summary>
    /// Изменяет описание задачи (операционное обновление)
    /// </summary>
    /// <param name="projectId">Идентификатор проекта</param>
    /// <param name="taskId">Идентификатор задачи</param>
    /// <param name="request">Новое описание задачи</param>
    /// <param name="handler">Обработчик команды ChangeTaskInfoCommand</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Результат обновления</returns>
    /// <response code="200">Описание успешно изменено</response>
    /// <response code="400">Некорректное описание</response>
    /// <response code="404">Задача или проект не найдены</response>
    [HttpPatch("{taskId}")]
    [ProducesResponseType(typeof(Envelope), 200)]
    [ProducesResponseType(typeof(Envelope), 400)]
    [ProducesResponseType(typeof(Envelope), 404)]
    public async Task<IActionResult> ChangeTask(
        Guid projectId,
        Guid taskId,
        [FromBody] ChangeTaskInfoRequest request,
        [FromServices] ChangeTaskInfoCommandHandler handler,
        CancellationToken ct)
    {
        var command = new ChangeTaskInfoCommand(
            projectId,
            taskId,
            request.Description
        );

        await handler.Handle(command, ct);
        return Ok(Envelope.Ok("Task updated"));
    }

    // -------------------------
    // ADD MEMBER
    // -------------------------
    /// <summary>
    /// Добавляет участника в задачу
    /// </summary>
    /// <param name="projectId">Идентификатор проекта</param>
    /// <param name="taskId">Идентификатор задачи</param>
    /// <param name="request">Данные участника (email и логин)</param>
    /// <param name="handler">Обработчик команды AddTaskMemberCommand</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Результат добавления</returns>
    /// <response code="200">Участник успешно добавлен</response>
    /// <response code="400">Некорректные данные участника</response>
    /// <response code="404">Задача или проект не найдены</response>
    [HttpPost("{taskId}/members")]
    [ProducesResponseType(typeof(Envelope), 200)]
    [ProducesResponseType(typeof(Envelope), 400)]
    [ProducesResponseType(typeof(Envelope), 404)]
    public async Task<IActionResult> AddMember(
        Guid projectId,
        Guid taskId,
        [FromBody] AddTaskMemberRequest request,
        [FromServices] AddTaskMemberCommandHandler handler,
        CancellationToken ct)
    {
        var command = new AddTaskMemberCommand(
            projectId,
            taskId,
            request.Email,
            request.Login
        );

        await handler.Handle(command, ct);
        return Ok(Envelope.Ok("Member added"));
    }

    // -------------------------
    // REMOVE MEMBER
    // -------------------------
    /// <summary>
    /// Удаляет участника из задачи
    /// </summary>
    /// <param name="projectId">Идентификатор проекта</param>
    /// <param name="taskId">Идентификатор задачи</param>
    /// <param name="memberId">Идентификатор участника</param>
    /// <param name="handler">Обработчик команды RemoveTaskMemberCommand</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Результат удаления</returns>
    /// <response code="200">Участник успешно удалён</response>
    /// <response code="404">Участник, задача или проект не найдены</response>
    [HttpDelete("{taskId}/members/{memberId}")]
    [ProducesResponseType(typeof(Envelope), 200)]
    [ProducesResponseType(typeof(Envelope), 404)]
    public async Task<IActionResult> RemoveMember(
        Guid projectId,
        Guid taskId,
        Guid memberId,
        [FromServices] RemoveTaskMemberCommandHandler handler,
        CancellationToken ct)
    {
        var command = new RemoveTaskMemberCommand(
            projectId,
            taskId,
            memberId
        );

        await handler.Handle(command, ct);
        return Ok(Envelope.Ok("Member removed"));
    }
}