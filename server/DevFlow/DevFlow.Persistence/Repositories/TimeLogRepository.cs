using DevFlow.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using TimeLogEntity = DevFlow.Domain.Entities.TimeLog;

namespace DevFlow.Persistence.Repositories;

public sealed class TimeLogRepository : ITimeLogRepository
{
    private readonly DevFlowDbContext _dbContext;

    public TimeLogRepository(DevFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async System.Threading.Tasks.Task<TimeLogEntity?> GetActiveByTaskIdAsync(int taskId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.TimeLogs
            .FirstOrDefaultAsync(timeLog => timeLog.TaskId == taskId && timeLog.EndTime == null, cancellationToken);
    }

    public async System.Threading.Tasks.Task AddAsync(TimeLogEntity timeLog, CancellationToken cancellationToken = default)
    {
        _dbContext.TimeLogs.Add(timeLog);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async System.Threading.Tasks.Task UpdateAsync(TimeLogEntity timeLog, CancellationToken cancellationToken = default)
    {
        _dbContext.TimeLogs.Update(timeLog);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

