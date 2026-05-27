using DevFlow.Domain.Enums;
using DevFlow.Domain.Interfaces;
using DevFlow.Domain.ValueObjects;

namespace DevFlow.Domain.Services;

public sealed class VersioningEngine : IVersioningEngine
{
    public SemanticVersion CalculateNextVersion(SemanticVersion currentVersion, VersionBumpType bumpType)
    {
        if (currentVersion is null)
        {
            throw new ArgumentNullException(nameof(currentVersion));
        }

        return bumpType switch
        {
            VersionBumpType.Major => new SemanticVersion(currentVersion.Major + 1, 0, 0),
            VersionBumpType.Minor => new SemanticVersion(currentVersion.Major, currentVersion.Minor + 1, 0),
            VersionBumpType.Patch => new SemanticVersion(currentVersion.Major, currentVersion.Minor, currentVersion.Patch + 1),
            _ => throw new ArgumentOutOfRangeException(nameof(bumpType), bumpType, null)
        };
    }
}

