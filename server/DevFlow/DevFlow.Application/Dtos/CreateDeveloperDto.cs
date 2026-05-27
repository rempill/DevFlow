namespace DevFlow.Application.Dtos;

public sealed record CreateDeveloperDto(
    string Name,
    string GitHubUser,
    string GitHubToken);

