using System.ComponentModel.DataAnnotations;
using DevFlow.Domain.Enums;

namespace DevFlow.Domain.Entities;

public class LeadDeveloper : Developer
{
    [Range(0, int.MaxValue)]
    public int PrivilegeLevel { get; private set; }

    protected LeadDeveloper()
    {
    }

    public LeadDeveloper(string name, string gitHubUser, string gitHubToken, int privilegeLevel)
        : base(name, gitHubUser, gitHubToken, DeveloperRole.LeadDeveloper)
    {
        PrivilegeLevel = privilegeLevel;
    }
}

