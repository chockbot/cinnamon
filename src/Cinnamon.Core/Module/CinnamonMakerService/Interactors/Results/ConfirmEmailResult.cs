using Microsoft.AspNetCore.Identity;

namespace Cinnamon.Core.Module.CinnamonMakerService.Interactors.Results;

public class ConfirmEmailResult 
{
    public IdentityUser User { get; set; }
}