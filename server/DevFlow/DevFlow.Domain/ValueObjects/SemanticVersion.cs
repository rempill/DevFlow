using System.ComponentModel.DataAnnotations;

namespace DevFlow.Domain.ValueObjects;

public sealed record SemanticVersion
{
    [Range(0, int.MaxValue)]
    public int Major { get; private init; }

    [Range(0, int.MaxValue)]
    public int Minor { get; private init; }

    [Range(0, int.MaxValue)]
    public int Patch { get; private init; }

    public SemanticVersion(int major, int minor, int patch)
    {
        Major = major;
        Minor = minor;
        Patch = patch;
    }

    public override string ToString()
    {
        return $"v{Major}.{Minor}.{Patch}";
    }
}

