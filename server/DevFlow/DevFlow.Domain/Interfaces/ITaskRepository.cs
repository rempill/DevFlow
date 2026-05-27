using TaskEntity = DevFlow.Domain.Entities.Task;

namespace DevFlow.Domain.Interfaces;

public interface ITaskRepository
{
    Task<TaskEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(TaskEntity task, CancellationToken cancellationToken = default);
    Task UpdateAsync(TaskEntity task, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TaskEntity>> GetByProjectIdAsync(int projectId, CancellationToken cancellationToken = default);
    Task<TaskEntity?> GetWithPrecursorsAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TaskEntity>> GetByProjectIdWithTimeLogsAsync(int projectId, CancellationToken cancellationToken = default);
}
