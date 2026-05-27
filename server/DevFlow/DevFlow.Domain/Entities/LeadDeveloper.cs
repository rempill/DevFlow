using System.ComponentModel.DataAnnotations;

namespace DevFlow.Domain.Entities;

public class LeadDeveloper : Developer
{
    [Range(0, int.MaxValue)]
    public int PrivilegeLevel { get; private set; }

    protected LeadDeveloper()
    {
    }

    public LeadDeveloper(string name, string gitHubUser, string gitHubToken, int privilegeLevel)
        : base(name, gitHubUser, gitHubToken)
    {
        PrivilegeLevel = privilegeLevel;
    }
}

