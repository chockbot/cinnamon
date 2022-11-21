using Cinnamon.Core.Common;
using Cinnamon.Core.Module.ActivityService.Interactors;
using Cinnamon.Core.Module.ActivityService.Interactors.Results;

namespace Cinnamon.Core.Module.ActivityService.Handler.UpdateActivityHandler;

public class UpdateActivityHandler : IUpdateActivityHandler
{
    public AppResult<UpdateActivityResult> Execute(UpdateActivity args)
    {
        try 
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<UpdateActivityResult>.CreateFailed(ex, "An error occured in UpdateActivityHandler.");
        }
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

            var update = await CoreDI.DataStore.Activities.SlowUpdateActivityAsync(args.Activity);
            if(update == null)
            {
                return AppResult<UpdateActivityResult>.CreateFailed(new ApplicationException("An error occured when updating activity"), "An error occured when updating activity");
            }

            var updatedActivity = await CoreDI.DataStore.Activities.GetActivityByIdAsync(args.Activity.Id);
            if(updatedActivity == null)
            {
                return AppResult<UpdateActivityResult>.CreateFailed(new ApplicationException("Can't find updated activity"), "Can't find updated activity");
            }

            return AppResult<UpdateActivityResult>.CreateSucceeded(new UpdateActivityResult {Activity = updatedActivity}, "Activity successfully updated.");
        }
        catch (Exception ex)
        {
            return AppResult<UpdateActivityResult>.CreateFailed(ex, "An error occured in UpdateActivityHandler.");
        }
    }
} 