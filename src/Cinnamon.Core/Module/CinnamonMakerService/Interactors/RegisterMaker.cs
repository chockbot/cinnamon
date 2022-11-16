using System;
using Cinnamon.Core.Interactor;

namespace Cinnamon.Core.Module.CinnamonMakerService.Interactors;

public class RegisterMaker : IInteractor 
{
    public string Password { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birthdate { get; set; }
    public bool AcceptFlag { get; set; }
}