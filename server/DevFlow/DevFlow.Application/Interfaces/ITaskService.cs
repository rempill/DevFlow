using DevFlow.Application.Dtos;
using DevFlow.Domain.Enums;
using TaskEntity = DevFlow.Domain.Entities.Task;

namespace DevFlow.Application.Interfaces;

public interface ITaskService
{
    Task<TaskEntity> CreateTaskAsync(int projectId, TaskDto dto, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProjectTaskDto>> GetProjectTasksAsync(int projectId, CancellationToken cancellationToken = default);
    Task<TaskEntity> StartTaskAsync(int taskId, CancellationToken cancellationToken = default);
    Task<TaskEntity> CompleteTaskAsync(int taskId, CancellationToken cancellationToken = default);
    Task<TaskEntity> TogglePhaseAsync(int taskId, PhaseType newPhase, CancellationToken cancellationToken = default);
    Task<TaskEntity> AddDependencyAsync(int taskId, int precursorId, CancellationToken cancellationToken = default);
    Task<TaskEntity> ApproveTaskAsync(int taskId, int approverId, CancellationToken cancellationToken = default);
}
