using TimeLogEntity = DevFlow.Domain.Entities.TimeLog;

namespace DevFlow.Domain.Interfaces;

public interface ITimeLogRepository
{
    System.Threading.Tasks.Task<TimeLogEntity?> GetActiveByTaskIdAsync(int taskId, CancellationToken cancellationToken = default);
    System.Threading.Tasks.Task AddAsync(TimeLogEntity timeLog, CancellationToken cancellationToken = default);
    System.Threading.Tasks.Task UpdateAsync(TimeLogEntity timeLog, CancellationToken cancellationToken = default);
}

