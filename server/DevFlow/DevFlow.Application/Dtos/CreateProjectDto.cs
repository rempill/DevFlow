namespace DevFlow.Application.Dtos;

public sealed record CreateProjectDto(
    int OwnerId,
    string RepoName,
    string Branch);

