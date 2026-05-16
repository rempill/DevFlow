using System.ComponentModel.DataAnnotations;
using DevFlow.Domain.Enums;

namespace DevFlow.Domain.Entities;

public class Review
{
    [Key]
    public int Id { get; private set; }

    public ReviewStatus Status { get; private set; }

    [MaxLength(2000)]
    public string? Comment { get; private set; }

    [Required]
    [MaxLength(40)]
    public string CommitSha { get; private set; }

    [Required]
    public Commit Commit { get; private set; } = null!;

    [Required]
    public int ReviewerId { get; private set; }

    [Required]
    public LeadDeveloper Reviewer { get; private set; } = null!;

    protected Review()
    {
        CommitSha = string.Empty;
    }

    public Review(Commit commit, LeadDeveloper reviewer, ReviewStatus status, string? comment)
    {
        Commit = commit ?? throw new ArgumentNullException(nameof(commit));
        CommitSha = commit.Sha;
        Reviewer = reviewer ?? throw new ArgumentNullException(nameof(reviewer));
        ReviewerId = reviewer.Id;
        Status = status;
        Comment = comment;
    }
}

