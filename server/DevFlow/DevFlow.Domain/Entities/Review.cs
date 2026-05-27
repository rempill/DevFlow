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
    public int ReviewerId { get; private set; }

    [Required]
    public LeadDeveloper Reviewer { get; private set; } = null!;

    protected Review()
    {
        CommitSha = string.Empty;
    }

    public Review(string commitSha, LeadDeveloper reviewer, ReviewStatus status, string? comment)
    {
        CommitSha = commitSha ?? throw new ArgumentNullException(nameof(commitSha));
        Reviewer = reviewer ?? throw new ArgumentNullException(nameof(reviewer));
        ReviewerId = reviewer.Id;
        Status = status;
        Comment = comment;
    }
}

