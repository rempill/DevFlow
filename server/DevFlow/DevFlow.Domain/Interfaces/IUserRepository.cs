using DevFlow.Domain.Entities;

namespace DevFlow.Domain.Interfaces;

public interface IUserRepository
{
    Task<Developer?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    System.Threading.Tasks.Task AddAsync(Developer user, CancellationToken cancellationToken = default);
    System.Threading.Tasks.Task UpdateAsync(Developer user, CancellationToken cancellationToken = default);
}

