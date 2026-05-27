using DevFlow.Application.Dtos;

namespace DevFlow.Application.Interfaces;

public interface IReportService
{
    Task<ProjectReportDto> GenerateAnalyticsAsync(int projectId, CancellationToken cancellationToken = default);
}

