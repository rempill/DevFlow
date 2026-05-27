using System.Text.RegularExpressions;
using DevFlow.Application.Interfaces;
using DevFlow.Domain.Enums;
using DevFlow.Domain.Interfaces;
using DevFlow.Domain.ValueObjects;

namespace DevFlow.Application.Services;

public sealed class DiagramService : IDiagramService
{
    private static readonly Regex VersionRegex = new(@"v(?<major>\d+)\.(?<minor>\d+)\.(?<patch>\d+)", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
    private const string VersionHeaderPrefix = "<!-- DevFlow Version: ";
    private const string VersionHeaderSuffix = " -->";

    private readonly IProjectRepository _projectRepository;
    private readonly IGitHubAdapter _gitHubAdapter;
    private readonly IVersioningEngine _versioningEngine;

    public DiagramService(
        IProjectRepository projectRepository,
        IGitHubAdapter gitHubAdapter,
        IVersioningEngine versioningEngine)
    {
        _projectRepository = projectRepository;
        _gitHubAdapter = gitHubAdapter;
        _versioningEngine = versioningEngine;
    }

    public async Task<(string Version, string CommitSha)> UploadDiagramAsync(int projectId, string fileName, string fileContent, VersionBumpType bumpType, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("FileName is required.", nameof(fileName));
        }

        if (string.IsNullOrWhiteSpace(fileContent))
        {
            throw new ArgumentException("Content is required.", nameof(fileContent));
        }

        var project = await _projectRepository.GetByIdAsync(projectId, cancellationToken);
        if (project is null)
        {
            throw new KeyNotFoundException($"Project {projectId} was not found.");
        }

        var existingFile = await _gitHubAdapter.GetFileContentAsync(project.Owner.GitHubUser, project.RepoName, fileName, cancellationToken);
        var currentVersion = existingFile is null
            ? new SemanticVersion(0, 0, 0)
            : ParseVersion(existingFile.Value.Content) ?? new SemanticVersion(0, 0, 0);

        var nextVersion = _versioningEngine.CalculateNextVersion(currentVersion, bumpType);
        var contentToStore = AddVersionHeader(fileContent, nextVersion);
        var commitMessage = $"Update {fileName} to {nextVersion}";

        var commitSha = await _gitHubAdapter.CreateOrUpdateFileAsync(
            project.Owner.GitHubUser,
            project.RepoName,
            fileName,
            contentToStore,
            commitMessage,
            existingFile?.Sha,
            cancellationToken);

        return (nextVersion.ToString(), commitSha);
    }

    private static SemanticVersion? ParseVersion(string content)
    {
        var match = VersionRegex.Match(content);
        if (!match.Success)
        {
            return null;
        }

        return new SemanticVersion(
            int.Parse(match.Groups["major"].Value),
            int.Parse(match.Groups["minor"].Value),
            int.Parse(match.Groups["patch"].Value));
    }

    private static string AddVersionHeader(string content, SemanticVersion version)
    {
        var header = $"{VersionHeaderPrefix}{version}{VersionHeaderSuffix}";
        return $"{header}\n{content}";
    }
}


