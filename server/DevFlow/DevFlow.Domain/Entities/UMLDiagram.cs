using System.ComponentModel.DataAnnotations;

namespace DevFlow.Domain.Entities;

public class UMLDiagram
{
    [Key]
    public int Id { get; private set; }

    [Required]
    [MaxLength(500)]
    public string FilePath { get; private set; }

    [MaxLength(40)]
    public string? LastSha { get; private set; }

    [Required]
    public int ProjectId { get; private set; }

    [Required]
    public Project Project { get; private set; } = null!;

    protected UMLDiagram()
    {
        FilePath = string.Empty;
    }

    public UMLDiagram(Project project, string filePath, string? lastSha)
    {
        Project = project ?? throw new ArgumentNullException(nameof(project));
        ProjectId = project.Id;
        FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        LastSha = lastSha;
    }
}

