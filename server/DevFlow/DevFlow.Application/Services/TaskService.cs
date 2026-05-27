using System.ComponentModel.DataAnnotations;
using DevFlow.Application.Dtos;
using DevFlow.Application.Interfaces;
using TaskStatusEnum = DevFlow.Domain.Enums.TaskStatus;
using DevFlow.Domain.Entities;
using DevFlow.Domain.Enums;
using DevFlow.Domain.Interfaces;
using TaskEntity = DevFlow.Domain.Entities.Task;

namespace DevFlow.Application.Services;

public sealed class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ITimeLogRepository _timeLogRepository;
    private readonly IGitHubAdapter _gitHubAdapter;
    private readonly IDependencyEngine _dependencyEngine;

    public TaskService(
        ITaskRepository taskRepository,
        IProjectRepository projectRepository,
        ITimeLogRepository timeLogRepository,
        IGitHubAdapter gitHubAdapter,
        IDependencyEngine dependencyEngine)
    {
        _taskRepository = taskRepository;
        _projectRepository = projectRepository;
        _timeLogRepository = timeLogRepository;
        _gitHubAdapter = gitHubAdapter;
        _dependencyEngine = dependencyEngine;
    }

    public async Task<TaskEntity> CreateTaskAsync(int projectId, TaskDto dto, CancellationToken cancellationToken = default)
    {
        if (dto is null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        var project = await _projectRepository.GetByIdAsync(projectId, cancellationToken);
        if (project is null)
        {
            throw new KeyNotFoundException($"Project {projectId} was not found.");
        }

        var issue = await _gitHubAdapter.CreateIssueAsync(
            project.Owner.GitHubUser,
            project.RepoName,
            dto.Title,
            dto.Body ?? string.Empty,
            cancellationToken);

        var status = dto.Status == default ? TaskStatusEnum.ToDo : dto.Status;
        var task = new TaskEntity(issue.Id, dto.Title, status, dto.IsBlocked, project);

        await _taskRepository.AddAsync(task, cancellationToken);

        return task;
    }

    public async Task<IReadOnlyList<ProjectTaskDto>> GetProjectTasksAsync(int projectId, CancellationToken cancellationToken = default)
    {
        var tasks = await _taskRepository.GetByProjectIdAsync(projectId, cancellationToken);
        if (tasks.Count == 0)
        {
            return Array.Empty<ProjectTaskDto>();
        }

        var taskDtos = tasks.Select(async task =>
        {
            var isBlocked = await _dependencyEngine.IsTaskBlockedAsync(task.Id, cancellationToken);
            return new ProjectTaskDto(
                task.Id,
                task.GitHubIssueId,
                task.Title,
                task.Status,
                isBlocked);
        });

        return await System.Threading.Tasks.Task.WhenAll(taskDtos);
    }

    public async Task<TaskEntity> StartTaskAsync(int taskId, CancellationToken cancellationToken = default)
    {
        var isBlocked = await _dependencyEngine.IsTaskBlockedAsync(taskId, cancellationToken);
        if (isBlocked)
        {
            throw new ValidationException($"Task {taskId} is blocked by open precursors.");
        }

        var task = await _taskRepository.GetByIdAsync(taskId, cancellationToken);
        if (task is null)
        {
            throw new KeyNotFoundException($"Task {taskId} was not found.");
        }

        task.MarkInProgress();
        await _taskRepository.UpdateAsync(task, cancellationToken);

        return task;
    }

    public async Task<TaskEntity> CompleteTaskAsync(int taskId, CancellationToken cancellationToken = default)
    {
        var task = await _taskRepository.GetWithPrecursorsAsync(taskId, cancellationToken);
        if (task is null)
        {
            throw new KeyNotFoundException($"Task {taskId} was not found.");
        }

        var activeTimeLog = await _timeLogRepository.GetActiveByTaskIdAsync(taskId, cancellationToken);
        if (activeTimeLog is not null)
        {
            activeTimeLog.Finish(DateTime.UtcNow);
            await _timeLogRepository.UpdateAsync(activeTimeLog, cancellationToken);
        }

        task.MarkPendingReview();
        await _taskRepository.UpdateAsync(task, cancellationToken);

        return task;
    }

    public async Task<TaskEntity> TogglePhaseAsync(int taskId, PhaseType newPhase, CancellationToken cancellationToken = default)
    {
        var task = await _taskRepository.GetByIdAsync(taskId, cancellationToken);
        if (task is null)
        {
            throw new KeyNotFoundException($"Task {taskId} was not found.");
        }

        var now = DateTime.UtcNow;
        var activeTimeLog = await _timeLogRepository.GetActiveByTaskIdAsync(taskId, cancellationToken);
        if (activeTimeLog is not null)
        {
            activeTimeLog.Finish(now);
            await _timeLogRepository.UpdateAsync(activeTimeLog, cancellationToken);
        }

        var nextLog = new TimeLog(taskId, newPhase, now);
        await _timeLogRepository.AddAsync(nextLog, cancellationToken);

        return task;
    }

    public async Task<TaskEntity> AddDependencyAsync(int taskId, int precursorId, CancellationToken cancellationToken = default)
    {
        var task = await _taskRepository.GetWithPrecursorsAsync(taskId, cancellationToken);
        if (task is null)
        {
            throw new KeyNotFoundException($"Task {taskId} was not found.");
        }

        var precursor = await _taskRepository.GetByIdAsync(precursorId, cancellationToken);
        if (precursor is null)
        {
            throw new KeyNotFoundException($"Task {precursorId} was not found.");
        }

        task.AddPrecursor(precursor);
        await _taskRepository.UpdateAsync(task, cancellationToken);

        return task;
    }
}
