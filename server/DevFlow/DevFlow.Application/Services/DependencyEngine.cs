using DevFlow.Domain.Enums;
using DevFlow.Domain.Interfaces;

namespace DevFlow.Application.Services;

public sealed class DependencyEngine : IDependencyEngine
{
    private readonly ITaskRepository _taskRepository;
    private readonly IGitHubAdapter _gitHubAdapter;

    public DependencyEngine(ITaskRepository taskRepository, IGitHubAdapter gitHubAdapter)
    {
        _taskRepository = taskRepository;
        _gitHubAdapter = gitHubAdapter;
    }

    public async Task<bool> IsTaskBlockedAsync(int taskId, CancellationToken cancellationToken = default)
    {
        var task = await _taskRepository.GetWithPrecursorsAsync(taskId, cancellationToken);
        if (task is null)
        {
            throw new KeyNotFoundException($"Task {taskId} was not found.");
        }

        if (task.Precursors.Count == 0)
        {
            return false;
        }

        var repoOwner = task.Project.Owner.GitHubUser;
        var repoName = task.Project.RepoName;

        foreach (var precursor in task.Precursors)
        {
            var status = await _gitHubAdapter.GetIssueStatusAsync(
                repoOwner,
                repoName,
                precursor.GitHubIssueId,
                cancellationToken);

            if (status == GitHubIssueState.Open)
            {
                return true;
            }
        }

        return false;
    }
}

