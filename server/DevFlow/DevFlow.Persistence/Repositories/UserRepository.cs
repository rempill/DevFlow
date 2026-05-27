using DevFlow.Domain.Entities;
using DevFlow.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.Persistence.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly DevFlowDbContext _dbContext;

    public UserRepository(DevFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Developer?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Developers
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
    }

    public async System.Threading.Tasks.Task AddAsync(Developer user, CancellationToken cancellationToken = default)
    {
        _dbContext.Developers.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async System.Threading.Tasks.Task UpdateAsync(Developer user, CancellationToken cancellationToken = default)
    {
        _dbContext.Developers.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

