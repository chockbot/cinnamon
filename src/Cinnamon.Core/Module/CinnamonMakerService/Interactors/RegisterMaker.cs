using System;
using Cinnamon.Core.Interactor;

namespace Cinnamon.Core.Module.CinnamonMakerService.Interactors;

public class RegisterMaker : IInteractor 
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
}