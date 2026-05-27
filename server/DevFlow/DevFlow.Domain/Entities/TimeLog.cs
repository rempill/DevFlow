using System.ComponentModel.DataAnnotations;
using DevFlow.Domain.Enums;

namespace DevFlow.Domain.Entities;

public class TimeLog
{
    [Key]
    public int Id { get; private set; }

    public PhaseType Phase { get; private set; }

    public DateTime StartTime { get; private set; }

    public DateTime? EndTime { get; private set; }

    [Required]
    public int TaskId { get; private set; }

    public Task Task { get; private set; } = null!;

    protected TimeLog()
    {
    }

    public TimeLog(int taskId, PhaseType phase, DateTime startTime, DateTime? endTime = null)
    {
        TaskId = taskId;
        Phase = phase;
        StartTime = startTime;
        EndTime = endTime;
    }

    public void Finish(DateTime endTime)
    {
        EndTime = endTime;
    }
}


