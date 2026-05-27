using DevFlow.Domain.Enums;
using DevFlow.Domain.ValueObjects;

namespace DevFlow.Domain.Interfaces;

public interface IVersioningEngine
{
	SemanticVersion CalculateNextVersion(SemanticVersion currentVersion, VersionBumpType bumpType);
}

