using Cinnamon.Core.Common;
using Cinnamon.Core.Module.ActivityService.Interactors;
using Cinnamon.Core.Module.ActivityService.Interactors.Results;

namespace Cinnamon.Core.Module.ActivityService.Handler.PublishActivity;

public class PublishActivityHandler : IPublishActivityHandler
{
    public AppResult<PublishResult> Execute(Publish args)
    {
        throw new NotImplementedException();
    }

    public async Task<AppResult<PublishResult>> ExecuteAsync(Publish args)
    {
        try
        {
            // check Activity if exist
            var model = new ActivityModel { Id = args.ActivityId };
            var activity = await CoreDI.DataStore.Activities.GetDataAsync(model);
            if(activity == null)
            {
                return AppResult<PublishResult>.CreateFailed(new ApplicationException("Can't find activity"), "Can't find activity");
            }

            activity.IsPublished = args.IsPublished;
            var res = await CoreDI.DataStore.Activities.SaveDataAsync(activity);
            if(!res.Message.ToLower().Contains("saved"))
            {
                return AppResult<PublishResult>.CreateFailed(new ApplicationException(res.Message), res.Message);
            }

            return AppResult<PublishResult>.CreateSucceeded(new PublishResult { Activity = activity }, "Activity successfully published/unpublish");
        }
        catch (Exception ex)
        {
            return AppResult<PublishResult>.CreateFailed(ex, "An error occured in PublishActivityHandler.");
        }
    }
} 