using DevFlow.Application.DependencyInjection;
using DevFlow.Application.Dtos;
using DevFlow.Application.Interfaces;
using DevFlow.Domain.Entities;
using DevFlow.Domain.Interfaces;
using DevFlow.Infrastructure.DependencyInjection;
using DevFlow.Persistence.DependencyInjection;
using DevFlow.Persistence.Seeding;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DevFlow");
if (string.IsNullOrWhiteSpace(connectionString))
{
	throw new InvalidOperationException("Connection string 'DevFlow' is not configured.");
}

builder.Services.AddPersistence(connectionString);
builder.Services.AddApplicationServices();
builder.Services.AddGitHubAdapter(
	builder.Configuration["GitHub:ProductName"] ?? "DevFlow",
	Environment.GetEnvironmentVariable("GITHUB_TOKEN"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DevFlow.Persistence.DevFlowDbContext>();
    await DevFlowSeeder.SeedAsync(dbContext);
}

app.MapGet("/", () => "Hello World!");

app.MapPost("/projects/{projectId:int}/tasks", async (
	int projectId,
	TaskDto dto,
	ITaskService taskService,
	CancellationToken cancellationToken) =>
{
	var task = await taskService.CreateTaskAsync(projectId, dto, cancellationToken);
	return Results.Created($"/projects/{projectId}/tasks/{task.Id}", new
	{
		task.Id,
		task.GitHubIssueId,
		task.Title,
		task.Status,
		task.IsBlocked
	});
});

app.MapGet("/projects/{projectId:int}/tasks", async (
	int projectId,
	ITaskService taskService,
	CancellationToken cancellationToken) =>
{
	var tasks = await taskService.GetProjectTasksAsync(projectId, cancellationToken);
	return Results.Ok(tasks);
});

app.MapGet("/tasks/{id:int}", async (
    int id,
    ITaskRepository taskRepository,
    CancellationToken cancellationToken) =>
{
    var task = await taskRepository.GetByIdAsync(id, cancellationToken);
    if (task is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(new
    {
        task.Id,
        task.GitHubIssueId,
        task.Title,
        task.Status,
        task.IsBlocked
    });
});

app.MapPost("/tasks/{id:int}/start", async (
    int id,
    ITaskService taskService,
    CancellationToken cancellationToken) =>
{
    var task = await taskService.StartTaskAsync(id, cancellationToken);
    return Results.Ok(new
    {
        task.Id,
        task.GitHubIssueId,
        task.Title,
        task.Status,
        task.IsBlocked
    });
});

app.MapPost("/tasks/{id:int}/complete", async (
    int id,
    ITaskService taskService,
    CancellationToken cancellationToken) =>
{
    var task = await taskService.CompleteTaskAsync(id, cancellationToken);
    return Results.Ok(new
    {
        task.Id,
        task.GitHubIssueId,
        task.Title,
        task.Status,
        task.IsBlocked
    });
});

app.MapPost("/tasks/{id:int}/dependencies/{precursorId:int}", async (
    int id,
    int precursorId,
    ITaskService taskService,
    CancellationToken cancellationToken) =>
{
    var task = await taskService.AddDependencyAsync(id, precursorId, cancellationToken);
    return Results.Ok(new
    {
        task.Id,
        task.GitHubIssueId,
        task.Title,
        task.Status,
        task.IsBlocked
    });
});

app.MapPost("/developers", async (
    CreateDeveloperDto dto,
    IUserRepository userRepository,
    CancellationToken cancellationToken) =>
{
    var developer = new Developer(dto.Name, dto.GitHubUser, dto.GitHubToken);
    await userRepository.AddAsync(developer, cancellationToken);

    return Results.Created($"/developers/{developer.Id}", new
    {
        developer.Id,
        developer.Name,
        developer.GitHubUser
    });
});

app.MapPost("/lead-developers", async (
    CreateLeadDeveloperDto dto,
    IUserRepository userRepository,
    CancellationToken cancellationToken) =>
{
    var developer = new LeadDeveloper(dto.Name, dto.GitHubUser, dto.GitHubToken, dto.PrivilegeLevel);
    await userRepository.AddAsync(developer, cancellationToken);

    return Results.Created($"/lead-developers/{developer.Id}", new
    {
        developer.Id,
        developer.Name,
        developer.GitHubUser,
        developer.PrivilegeLevel
    });
});

app.MapGet("/developers/{id:int}", async (
    int id,
    IUserRepository userRepository,
    CancellationToken cancellationToken) =>
{
    var developer = await userRepository.GetByIdAsync(id, cancellationToken);
    if (developer is null)
    {
        return Results.NotFound();
    }

    if (developer is LeadDeveloper leadDeveloper)
    {
        return Results.Ok(new
        {
            leadDeveloper.Id,
            leadDeveloper.Name,
            leadDeveloper.GitHubUser,
            leadDeveloper.PrivilegeLevel
        });
    }

    return Results.Ok(new
    {
        developer.Id,
        developer.Name,
        developer.GitHubUser
    });
});

app.MapPost("/projects", async (
    CreateProjectDto dto,
    IUserRepository userRepository,
    IProjectRepository projectRepository,
    CancellationToken cancellationToken) =>
{
    var owner = await userRepository.GetByIdAsync(dto.OwnerId, cancellationToken);
    if (owner is null)
    {
        return Results.NotFound(new { Message = $"Owner {dto.OwnerId} was not found." });
    }

    var project = new Project(owner, dto.RepoName, dto.Branch);
    await projectRepository.AddAsync(project, cancellationToken);

    return Results.Created($"/projects/{project.Id}", new
    {
        project.Id,
        project.OwnerId,
        project.RepoName,
        project.Branch
    });
});

app.MapGet("/projects/{id:int}", async (
    int id,
    IProjectRepository projectRepository,
    CancellationToken cancellationToken) =>
{
    var project = await projectRepository.GetByIdAsync(id, cancellationToken);
    if (project is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(new
    {
        project.Id,
        project.OwnerId,
        Owner = new
        {
            project.Owner.Id,
            project.Owner.Name,
            project.Owner.GitHubUser
        },
        project.RepoName,
        project.Branch,
        UmlDiagram = project.UMLDiagram is null ? null : new
        {
            project.UMLDiagram.Id,
            project.UMLDiagram.FilePath,
            project.UMLDiagram.LastSha
        }
    });
});

app.Run();
