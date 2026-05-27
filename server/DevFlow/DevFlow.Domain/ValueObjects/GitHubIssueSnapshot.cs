using System.ComponentModel.DataAnnotations;

namespace DevFlow.Domain.ValueObjects;

public sealed record GitHubIssueSnapshot
{
    [Range(1, long.MaxValue)]
    public long Id { get; private init; }

    [Required]
    public string Title { get; private init; }

    public IReadOnlyCollection<string> Labels { get; private init; }

    public GitHubIssueSnapshot(long id, string title, IReadOnlyCollection<string> labels)
    {
        Id = id;
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Labels = labels ?? Array.Empty<string>();
    }
}

