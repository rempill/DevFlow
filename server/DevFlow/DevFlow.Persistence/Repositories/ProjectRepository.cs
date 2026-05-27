using DevFlow.Domain.Entities;
using DevFlow.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.Persistence.Repositories;

public sealed class ProjectRepository : IProjectRepository
{
    private readonly DevFlowDbContext _dbContext;

    public ProjectRepository(DevFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Project?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Projects
            .Include(project => project.Owner)
            .Include(project => project.UMLDiagram)
            .FirstOrDefaultAsync(project => project.Id == id, cancellationToken);
    }

    public async System.Threading.Tasks.Task AddAsync(Project project, CancellationToken cancellationToken = default)
    {
        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async System.Threading.Tasks.Task UpdateAsync(Project project, CancellationToken cancellationToken = default)
    {
        _dbContext.Projects.Update(project);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

