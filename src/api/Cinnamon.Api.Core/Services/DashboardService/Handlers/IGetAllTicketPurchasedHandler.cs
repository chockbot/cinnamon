using Cinnamon.Api.Core.Services.DashboardService.Interactors;
using Cinnamon.Api.Core.Services.DashboardService.Interactors.Results;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.DashboardService.Handlers;

public interface IGetAllTicketPurchasedHandler : IInteractorHandler<GetAllTicketPurchasedArgs, AppResult<GetAllTicketPurchasedResult>>
{
}
