using System.ComponentModel.DataAnnotations;

namespace DevFlow.Domain.Entities;

public class Project
{
    [Key]
    public int Id { get; private set; }

    [Required]
    public int OwnerId { get; private set; }

    [Required]
    public Developer Owner { get; private set; } = null!;

    [Required]
    [MaxLength(200)]
    public string RepoName { get; private set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Branch { get; private set; } = string.Empty;

    public ICollection<Task> Tasks { get; private set; } = new HashSet<Task>();

    protected Project()
    {
    }

    public Project(Developer owner, string repoName, string branch)
    {
        Owner = owner ?? throw new ArgumentNullException(nameof(owner));
        OwnerId = owner.Id;
        RepoName = repoName ?? throw new ArgumentNullException(nameof(repoName));
        Branch = branch ?? throw new ArgumentNullException(nameof(branch));
    }
}
