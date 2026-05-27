namespace DevFlow.Application.Dtos;

public sealed class ProjectReportDto
{
    public int TotalTasks { get; init; }
    public int BlockedTasksCount { get; init; }
    public double TotalHoursSpent { get; init; }
    public double AnalysisRatioPercentage { get; init; }
    public double ImplementationRatioPercentage { get; init; }
}

