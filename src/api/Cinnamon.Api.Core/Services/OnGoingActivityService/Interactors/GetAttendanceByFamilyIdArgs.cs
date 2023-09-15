using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors;

public class GetAttendanceByFamilyIdArgs : IInteractor
{
    public int FamilyId { get; set; }
}
