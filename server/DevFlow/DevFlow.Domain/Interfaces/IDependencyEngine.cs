namespace DevFlow.Domain.Interfaces;

public interface IDependencyEngine
{
    Task<bool> IsTaskBlockedAsync(int taskId, CancellationToken cancellationToken = default);
}

