using Microsoft.AspNetCore.Identity;
namespace Cinnamon.Core.Module.CinnamonMakerService.Interactors.Results;

public class RegisterMakerResult 
{
    public IdentityUser User { get; set; }
}