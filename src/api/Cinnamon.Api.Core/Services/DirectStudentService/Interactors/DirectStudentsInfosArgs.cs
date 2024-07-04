using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.DirectStudentService.Interactors;

public class DirectStudentsInfosArgs : IInteractor 
{
    public int PageIndex {get; set;}
    public int PageCount {get; set;}
    public string? SearchValue { get; set; }
}