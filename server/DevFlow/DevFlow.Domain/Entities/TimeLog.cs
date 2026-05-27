using System.ComponentModel.DataAnnotations;
using DevFlow.Domain.Enums;

namespace DevFlow.Domain.Entities;

public class TimeLog
{
    [Key]
    public int Id { get; private set; }

    [Required]
    public TimeSpan Duration { get; private set; }

    public PhaseType Phase { get; private set; }

    public DateTimeOffset StartTime { get; private set; }

    [Required]
    public int TaskId { get; private set; }

    [Required]
    public Task Task { get; private set; } = null!;

    protected TimeLog()
    {
    }

    public TimeLog(Task task, TimeSpan duration, PhaseType phase, DateTimeOffset startTime)
    {
        Task = task ?? throw new ArgumentNullException(nameof(task));
        TaskId = task.Id;
        Duration = duration;
        Phase = phase;
        StartTime = startTime;
    }
}


