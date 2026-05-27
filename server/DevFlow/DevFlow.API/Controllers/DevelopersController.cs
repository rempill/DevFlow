using DevFlow.Application.Dtos;
using DevFlow.Domain.Entities;
using DevFlow.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevFlow.API.Controllers;

[ApiController]
[Route("developers")]
public sealed class DevelopersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public DevelopersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateDeveloper([FromBody] CreateDeveloperDto dto, CancellationToken cancellationToken)
    {
        var developer = new Developer(dto.Name, dto.GitHubUser, dto.GitHubToken);
        await _userRepository.AddAsync(developer, cancellationToken);

        return Created($"/developers/{developer.Id}", MapDeveloper(developer));
    }

    [HttpPost("/lead-developers")]
    public async Task<IActionResult> CreateLeadDeveloper([FromBody] CreateLeadDeveloperDto dto, CancellationToken cancellationToken)
    {
        var developer = new LeadDeveloper(dto.Name, dto.GitHubUser, dto.GitHubToken, dto.PrivilegeLevel);
        await _userRepository.AddAsync(developer, cancellationToken);

        return Created($"/lead-developers/{developer.Id}", MapDeveloper(developer));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetDeveloper(int id, CancellationToken cancellationToken)
    {
        var developer = await _userRepository.GetByIdAsync(id, cancellationToken);
        return developer is null ? NotFound() : Ok(MapDeveloper(developer));
    }

    private static object MapDeveloper(Developer developer)
    {
        if (developer is LeadDeveloper leadDeveloper)
        {
            return new
            {
                leadDeveloper.Id,
                leadDeveloper.Name,
                leadDeveloper.GitHubUser,
                leadDeveloper.Role,
                leadDeveloper.PrivilegeLevel
            };
        }

        return new
        {
            developer.Id,
            developer.Name,
            developer.GitHubUser,
            developer.Role
        };
    }
}

