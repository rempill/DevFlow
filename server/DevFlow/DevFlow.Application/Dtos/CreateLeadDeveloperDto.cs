namespace DevFlow.Application.Dtos;

public sealed record CreateLeadDeveloperDto(
    string Name,
    string GitHubUser,
    string GitHubToken,
    int PrivilegeLevel);

