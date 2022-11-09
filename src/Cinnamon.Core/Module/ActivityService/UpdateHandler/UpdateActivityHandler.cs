using Cinnamon.Core.Common;
using Cinnamon.Core.Module.ActivityService.Interactors;
using Cinnamon.Core.Module.ActivityService.Interactors.Results;

namespace Cinnamon.Core.Module.ActivityService.Handler.UpdateActivityHandler;

public class UpdateActivityHandler : IUpdateActivityHandler
{
    public AppResult<UpdateActivityResult> Execute(UpdateActivity args)
    {
        throw new NotImplementedException();
    }

    public async Task<AppResult<UpdateActivityResult>> ExecuteAsync(UpdateActivity args)
    {
        try
        {
            // check if activity exist
            var res = await CoreDI.DataStore.Activities.GetDataAsync(new ActivityModel {Id = args.Activity.Id});
            if(res == null)
            {
                return AppResult<UpdateActivityResult>.CreateFailed(new ApplicationException("Can't find activity"), "Can't find activity");
            }

            var update = await CoreDI.DataStore.Activities.SaveDataAsync(args.Activity);
            if(!update.Message.ToLower().Contains("saved"))
            {
                return AppResult<UpdateActivityResult>.CreateFailed(new ApplicationException(update.Message), update.Message);
            }

            return AppResult<UpdateActivityResult>.CreateSucceeded(new UpdateActivityResult {Activity = args.Activity}, "Activity successfully updated.");
        }
        catch (Exception ex)
        {
            return AppResult<UpdateActivityResult>.CreateFailed(ex, "An error occured in UpdateActivityHandler.");
        }
    }
} 