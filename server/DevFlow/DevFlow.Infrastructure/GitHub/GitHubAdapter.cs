using DevFlow.Domain.Enums;
using DevFlow.Domain.Interfaces;
using DevFlow.Domain.ValueObjects;
using Octokit;

namespace DevFlow.Infrastructure.GitHub;

public sealed class GitHubAdapter : IGitHubAdapter
{
    private readonly GitHubClient _client;

    public GitHubAdapter(GitHubClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<GitHubIssueSnapshot> GetIssueAsync(
        string repoOwner,
        string repoName,
        long issueId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var issue = await _client.Issue.Get(repoOwner, repoName, checked((int)issueId));
        return new GitHubIssueSnapshot(
            issue.Number,
            issue.Title,
            issue.Labels.Select(label => label.Name).ToArray());
    }

    public async Task UpdateIssueLabelAsync(
        string repoOwner,
        string repoName,
        long issueId,
        string label,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await _client.Issue.Labels.AddToIssue(repoOwner, repoName, checked((int)issueId), new[] { label });
    }

    public async Task<GitHubIssueSnapshot> CreateIssueAsync(
        string repoOwner,
        string repoName,
        string title,
        string body,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var newIssue = new NewIssue(title)
        {
            Body = body
        };

        var issue = await _client.Issue.Create(repoOwner, repoName, newIssue);
        return new GitHubIssueSnapshot(
            issue.Number,
            issue.Title,
            issue.Labels.Select(label => label.Name).ToArray());
    }

    public async Task<GitHubIssueState> GetIssueStatusAsync(
        string repoOwner,
        string repoName,
        long issueId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var issue = await _client.Issue.Get(repoOwner, repoName, checked((int)issueId));
        return issue.State == ItemState.Open ? GitHubIssueState.Open : GitHubIssueState.Closed;
    }

    public async Task CloseIssueAsync(
        string repoOwner,
        string repoName,
        long issueId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var update = new IssueUpdate
        {
            State = ItemState.Closed
        };

        await _client.Issue.Update(repoOwner, repoName, checked((int)issueId), update);
    }
}
