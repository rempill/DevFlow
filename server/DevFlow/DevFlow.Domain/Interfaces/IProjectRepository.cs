using DevFlow.Domain.Entities;

namespace DevFlow.Domain.Interfaces;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    System.Threading.Tasks.Task AddAsync(Project project, CancellationToken cancellationToken = default);
    System.Threading.Tasks.Task UpdateAsync(Project project, CancellationToken cancellationToken = default);
}

