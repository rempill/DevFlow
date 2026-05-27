using DevFlow.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevFlow.API.Controllers;

[ApiController]
public sealed class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("/projects/{projectId:int}/report")]
    public async Task<IActionResult> GetProjectReport(int projectId, CancellationToken cancellationToken)
    {
        var report = await _reportService.GenerateAnalyticsAsync(projectId, cancellationToken);
        return Ok(report);
    }
}

