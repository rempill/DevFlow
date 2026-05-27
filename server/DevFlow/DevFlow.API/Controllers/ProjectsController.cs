using DevFlow.Application.Dtos;
using DevFlow.Application.Interfaces;
using DevFlow.Domain.Entities;
using DevFlow.Domain.Enums;
using DevFlow.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevFlow.API.Controllers;

[ApiController]
[Route("projects")]
public sealed class ProjectsController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IDiagramService _diagramService;

    public ProjectsController(IUserRepository userRepository, IProjectRepository projectRepository, IDiagramService diagramService)
    {
        _userRepository = userRepository;
        _projectRepository = projectRepository;
        _diagramService = diagramService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto dto, CancellationToken cancellationToken)
    {
        var owner = await _userRepository.GetByIdAsync(dto.OwnerId, cancellationToken);
        if (owner is null)
        {
            return NotFound(new { Message = $"Owner {dto.OwnerId} was not found." });
        }

        var project = new Project(owner, dto.RepoName, dto.Branch);
        await _projectRepository.AddAsync(project, cancellationToken);

        return Created($"/projects/{project.Id}", MapProject(project));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProject(int id, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(id, cancellationToken);
        return project is null ? NotFound() : Ok(MapProject(project));
    }

    [HttpPost("{projectId:int}/diagrams")]
    public async Task<IActionResult> UploadDiagram(int projectId, [FromBody] DiagramUploadDto dto, CancellationToken cancellationToken)
    {
        var result = await _diagramService.UploadDiagramAsync(projectId, dto.FileName, dto.Content, dto.BumpType, cancellationToken);

        return Created($"/projects/{projectId}/diagrams/{dto.FileName}", new
        {
            Version = result.Version,
            CommitSha = result.CommitSha
        });
    }

    private static object MapProject(Project project)
    {
        return new
        {
            project.Id,
            project.OwnerId,
            Owner = new
            {
                project.Owner.Id,
                project.Owner.Name,
                project.Owner.GitHubUser,
                project.Owner.Role
            },
            project.RepoName,
            project.Branch
        };
    }
}

