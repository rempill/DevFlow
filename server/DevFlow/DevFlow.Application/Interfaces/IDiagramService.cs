using DevFlow.Application.Dtos;
using DevFlow.Domain.Enums;

namespace DevFlow.Application.Interfaces;

public interface IDiagramService
{
    Task<(string Version, string CommitSha)> UploadDiagramAsync(int projectId, string fileName, string fileContent, VersionBumpType bumpType, CancellationToken cancellationToken = default);
}


