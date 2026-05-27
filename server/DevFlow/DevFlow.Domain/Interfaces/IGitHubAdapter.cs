using DevFlow.Domain.Enums;
using DevFlow.Domain.ValueObjects;

namespace DevFlow.Domain.Interfaces;

public interface IGitHubAdapter
{
    Task<GitHubIssueSnapshot> GetIssueAsync(string repoOwner, string repoName, long issueId, CancellationToken cancellationToken = default);
    Task UpdateIssueLabelAsync(string repoOwner, string repoName, long issueId, string label, CancellationToken cancellationToken = default);
    Task<GitHubIssueSnapshot> CreateIssueAsync(string repoOwner, string repoName, string title, string body, CancellationToken cancellationToken = default);
    Task<GitHubIssueState> GetIssueStatusAsync(string repoOwner, string repoName, long issueId, CancellationToken cancellationToken = default);
    Task CloseIssueAsync(string repoOwner, string repoName, long issueId, CancellationToken cancellationToken = default);
    Task<(string Content, string Sha)?> GetFileContentAsync(string owner, string repo, string path, CancellationToken cancellationToken = default);
    Task<string> CreateOrUpdateFileAsync(string owner, string repo, string path, string content, string commitMessage, string? sha, CancellationToken cancellationToken = default);
}
