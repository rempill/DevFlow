using System.ComponentModel.DataAnnotations;
using DevFlow.Domain.ValueObjects;

namespace DevFlow.Domain.Entities;

public class Commit
{
    [Key]
    [MaxLength(40)]
    public string Sha { get; private set; }

    [Required]
    [MaxLength(500)]
    public string Message { get; private set; }

    public DateTimeOffset Timestamp { get; private set; }

    [Required]
    public SemanticVersion Version { get; private set; }

    [Required]
    public int TaskId { get; private set; }

    [Required]
    public Task Task { get; private set; } = null!;

    public ICollection<Review> Reviews { get; private set; } = new HashSet<Review>();

    protected Commit()
    {
        Sha = string.Empty;
        Message = string.Empty;
        Version = new SemanticVersion(0, 0, 0);
    }

    public Commit(string sha, string message, DateTimeOffset timestamp, SemanticVersion version, Task task)
    {
        Sha = sha ?? throw new ArgumentNullException(nameof(sha));
        Message = message ?? throw new ArgumentNullException(nameof(message));
        Timestamp = timestamp;
        Version = version ?? throw new ArgumentNullException(nameof(version));
        Task = task ?? throw new ArgumentNullException(nameof(task));
        TaskId = task.Id;
    }
}


