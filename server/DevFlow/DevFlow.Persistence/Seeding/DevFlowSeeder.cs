using DevFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.Persistence.Seeding;

public static class DevFlowSeeder
{
    public static async System.Threading.Tasks.Task SeedAsync(DevFlowDbContext dbContext, CancellationToken cancellationToken = default)
    {
        await dbContext.Database.MigrateAsync(cancellationToken);

        var hasProjects = await dbContext.Projects.AnyAsync(cancellationToken);
        if (hasProjects)
        {
            return;
        }

        var owner = new LeadDeveloper(
            name: "rempill",
            gitHubUser: "rempill",
            gitHubToken: "dev-token",
            privilegeLevel: 1);

        var project = new Project(
            owner: owner,
            repoName: "devflow-test",
            branch: "main");

        dbContext.Developers.Add(owner);
        dbContext.Projects.Add(project);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
