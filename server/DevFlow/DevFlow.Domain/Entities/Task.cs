using System.ComponentModel.DataAnnotations;
using TaskStatusEnum = DevFlow.Domain.Enums.TaskStatus;

namespace DevFlow.Domain.Entities;

public class Task
{
    [Key]
    public int Id { get; private set; }

    [Range(1, long.MaxValue)]
    public long GitHubIssueId { get; private set; }

    [Required]
    [MaxLength(300)]
    public string Title { get; private set; } = string.Empty;

    public TaskStatusEnum Status { get; private set; }

    public bool IsBlocked { get; private set; }

    public ICollection<Task> Precursors { get; private set; } = new HashSet<Task>();

    public ICollection<Task> Successors { get; private set; } = new HashSet<Task>();

    public ICollection<TimeLog> TimeLogs { get; private set; } = new HashSet<TimeLog>();

    public ICollection<Commit> Commits { get; private set; } = new HashSet<Commit>();

    [Required]
    public int ProjectId { get; private set; }

    [Required]
    public Project Project { get; private set; } = null!;

    protected Task()
    {
    }

    public Task(long gitHubIssueId, string title, TaskStatusEnum status, bool isBlocked, Project project)
    {
        GitHubIssueId = gitHubIssueId;
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Status = status;
        IsBlocked = isBlocked;
        Project = project ?? throw new ArgumentNullException(nameof(project));
        ProjectId = project.Id;
    }

    public void MarkInProgress()
    {
        Status = TaskStatusEnum.InProgress;
    }

    public void MarkFinished()
    {
        Status = TaskStatusEnum.Finished;
    }

    public void AddPrecursor(Task precursor)
    {
        if (precursor is null)
        {
            throw new ArgumentNullException(nameof(precursor));
        }

        if (precursor.Id == Id || Precursors.Any(existing => existing.Id == precursor.Id))
        {
            return;
        }

        Precursors.Add(precursor);
        precursor.Successors.Add(this);
    }
}
