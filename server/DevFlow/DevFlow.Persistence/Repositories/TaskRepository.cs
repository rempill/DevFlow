using DevFlow.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using TaskEntity = DevFlow.Domain.Entities.Task;

namespace DevFlow.Persistence.Repositories;

public sealed class TaskRepository : ITaskRepository
{
    private readonly DevFlowDbContext _dbContext;

    public TaskRepository(DevFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TaskEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tasks.FirstOrDefaultAsync(task => task.Id == id, cancellationToken);
    }

    public async Task AddAsync(TaskEntity task, CancellationToken cancellationToken = default)
    {
        _dbContext.Tasks.Add(task);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(TaskEntity task, CancellationToken cancellationToken = default)
    {
        _dbContext.Tasks.Update(task);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<TaskEntity>> GetByProjectIdAsync(int projectId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tasks
            .Where(task => task.ProjectId == projectId)
            .ToListAsync(cancellationToken);
    }

    public async Task<TaskEntity?> GetWithPrecursorsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tasks
            .Include(task => task.Precursors)
            .Include(task => task.Project)
            .ThenInclude(project => project.Owner)
            .FirstOrDefaultAsync(task => task.Id == id, cancellationToken);
    }
}
