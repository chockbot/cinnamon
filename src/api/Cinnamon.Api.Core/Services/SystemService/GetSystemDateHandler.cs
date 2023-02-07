using Cinnamon.Api.Core.Services.SystemService.Handlers;
using Cinnamon.Api.Core.Services.SystemService.Interactors;
using Cinnamon.Api.Core.Services.SystemService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.SystemService;

public class GetSystemDateHandler : IGetSystemDateHandler
{
    public AppResult<GetSystemDateResult> Execute(GetSystemDateArgs args)
    {
        try
        {
            DateTime currentDate = DateTime.Now;

            return AppResult<GetSystemDateResult>.CreateSucceeded(new GetSystemDateResult {
                ServerDate = currentDate,
            }, "Successfully get server date");   
        }
        catch (Exception ex)
        {
            return AppResult<GetSystemDateResult>.CreateFailed(ex, "An error occured in GetSystemDateHandler");
        }
    }

    public Task<AppResult<GetSystemDateResult>> ExecuteAsync(GetSystemDateArgs args)
    {
        return Task.Run(() => Execute(args));
    }
}