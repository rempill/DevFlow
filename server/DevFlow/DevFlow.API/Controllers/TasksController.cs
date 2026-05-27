using DevFlow.Application.Dtos;
using DevFlow.Application.Interfaces;
using DevFlow.Domain.Enums;
using DevFlow.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using TaskEntity = DevFlow.Domain.Entities.Task;

namespace DevFlow.API.Controllers;

[ApiController]
public sealed class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly ITaskRepository _taskRepository;

    public TasksController(ITaskService taskService, ITaskRepository taskRepository)
    {
        _taskService = taskService;
        _taskRepository = taskRepository;
    }

    [HttpPost("/projects/{projectId:int}/tasks")]
    public async Task<IActionResult> CreateTask(int projectId, [FromBody] TaskDto dto, CancellationToken cancellationToken)
    {
        var task = await _taskService.CreateTaskAsync(projectId, dto, cancellationToken);
        return Created($"/projects/{projectId}/tasks/{task.Id}", MapTask(task));
    }

    [HttpGet("/projects/{projectId:int}/tasks")]
    public async Task<IActionResult> GetProjectTasks(int projectId, CancellationToken cancellationToken)
    {
        var tasks = await _taskService.GetProjectTasksAsync(projectId, cancellationToken);
        return Ok(tasks);
    }

    [HttpGet("/tasks/{id:int}")]
    public async Task<IActionResult> GetTask(int id, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(id, cancellationToken);
        return task is null ? NotFound() : Ok(MapTask(task));
    }

    [HttpPost("/tasks/{id:int}/start")]
    public async Task<IActionResult> StartTask(int id, CancellationToken cancellationToken)
    {
        var task = await _taskService.StartTaskAsync(id, cancellationToken);
        return Ok(MapTask(task));
    }

    [HttpPost("/tasks/{id:int}/complete")]
    public async Task<IActionResult> CompleteTask(int id, CancellationToken cancellationToken)
    {
        var task = await _taskService.CompleteTaskAsync(id, cancellationToken);
        return Ok(MapTask(task));
    }

    [HttpPost("/tasks/{id:int}/phase")]
    public async Task<IActionResult> TogglePhase(int id, [FromQuery] PhaseType newPhase, CancellationToken cancellationToken)
    {
        var task = await _taskService.TogglePhaseAsync(id, newPhase, cancellationToken);
        return Ok(MapTask(task));
    }

    [HttpPost("/tasks/{id:int}/dependencies/{precursorId:int}")]
    public async Task<IActionResult> AddDependency(int id, int precursorId, CancellationToken cancellationToken)
    {
        var task = await _taskService.AddDependencyAsync(id, precursorId, cancellationToken);
        return Ok(MapTask(task));
    }

    [HttpPost("/tasks/{id:int}/approve")]
    public async Task<IActionResult> ApproveTask(int id, [FromQuery] int approverId, CancellationToken cancellationToken)
    {
        var task = await _taskService.ApproveTaskAsync(id, approverId, cancellationToken);
        return Ok(MapTask(task));
    }

    private static object MapTask(TaskEntity task)
    {
        return new
        {
            task.Id,
            task.GitHubIssueId,
            task.Title,
            task.Status,
            task.IsBlocked
        };
    }
}

