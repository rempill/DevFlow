using TaskStatusEnum = DevFlow.Domain.Enums.TaskStatus;

namespace DevFlow.Application.Dtos;

public sealed record ProjectTaskDto(
    int Id,
    long GitHubIssueId,
    string Title,
    TaskStatusEnum Status,
    bool IsBlocked);

