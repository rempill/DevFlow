using DevFlow.Application.Dtos;
using DevFlow.Application.Interfaces;
using DevFlow.Domain.Entities;
using DevFlow.Domain.Enums;
using DevFlow.Domain.Interfaces;

namespace DevFlow.Application.Services;

public sealed class ReportService : IReportService
{
    private readonly ITaskRepository _taskRepository;

    public ReportService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<ProjectReportDto> GenerateAnalyticsAsync(int projectId, CancellationToken cancellationToken = default)
    {
        var tasks = await _taskRepository.GetByProjectIdWithTimeLogsAsync(projectId, cancellationToken);

        var totalTasks = tasks.Count;
        var blockedCount = tasks.Count(t => t.IsBlocked);

        // Flatten time logs across tasks
        var closedLogs = tasks.SelectMany(t => t.TimeLogs)
            .Where(l => l.EndTime.HasValue)
            .ToList();

        double totalSeconds = closedLogs.Sum(l => (l.EndTime!.Value - l.StartTime).TotalSeconds);
        double totalHours = totalSeconds / 3600.0;

        double analysisSeconds = closedLogs.Where(l => l.Phase == PhaseType.Analysis)
            .Sum(l => (l.EndTime!.Value - l.StartTime).TotalSeconds);

        double implementationSeconds = closedLogs.Where(l => l.Phase == PhaseType.Implementation)
            .Sum(l => (l.EndTime!.Value - l.StartTime).TotalSeconds);

        double analysisRatio = 0.0;
        double implementationRatio = 0.0;

        if (totalSeconds > 0)
        {
            analysisRatio = (analysisSeconds / totalSeconds) * 100.0;
            implementationRatio = (implementationSeconds / totalSeconds) * 100.0;
        }

        return new ProjectReportDto
        {
            TotalTasks = totalTasks,
            BlockedTasksCount = blockedCount,
            TotalHoursSpent = Math.Round(totalHours, 2),
            AnalysisRatioPercentage = Math.Round(analysisRatio, 2),
            ImplementationRatioPercentage = Math.Round(implementationRatio, 2)
        };
    }
}

