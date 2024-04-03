using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;
public class DeleteTicketHandler : IDeleteTicketHandler
{
    private readonly IActivityData activityData;
    public DeleteTicketHandler(IActivityData activityData)
    {
        this.activityData = activityData;
    }

    public AppResult<DeleteTicketResult> Execute(DeleteTicketArgs interactor)
    {
        throw new NotImplementedException();
    }

    public Task<AppResult<DeleteTicketResult>> ExecuteAsync(DeleteTicketArgs interactor)
    {
        throw new NotImplementedException();
    }
}
