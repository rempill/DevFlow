using System.ComponentModel.DataAnnotations;

namespace DevFlow.Domain.Entities;

public class Developer
{
    [Key]
    public int Id { get; private set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; private set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string GitHubUser { get; private set; } = string.Empty;

    [Required]
    public string GitHubToken { get; private set; } = string.Empty;

    protected Developer()
    {
    }

    public Developer(string name, string gitHubUser, string gitHubToken)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        GitHubUser = gitHubUser ?? throw new ArgumentNullException(nameof(gitHubUser));
        GitHubToken = gitHubToken ?? throw new ArgumentNullException(nameof(gitHubToken));
    }
}


