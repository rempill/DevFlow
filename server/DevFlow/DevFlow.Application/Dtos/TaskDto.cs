using TaskStatusEnum = DevFlow.Domain.Enums.TaskStatus;

namespace DevFlow.Application.Dtos;

public sealed record TaskDto(
    string Title,
    string? Body,
    TaskStatusEnum Status = TaskStatusEnum.ToDo,
    bool IsBlocked = false);


