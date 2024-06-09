using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.DirectStudentService.Interactors;

public class StudentSessionsArgs : IInteractor 
{
    public int StudentId {get; set;}
    public string? SessionStatus {get; set;}
}