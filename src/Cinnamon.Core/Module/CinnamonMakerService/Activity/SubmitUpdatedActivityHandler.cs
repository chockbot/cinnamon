using Cinnamon.Core.Models;
using Cinnamon.Core.Common;
using Cinnamon.Core.Module.CinnamonMakerService.Interactors;
using Cinnamon.Core.Module.CinnamonMakerService.Interactors.Results;
using Cinnamon.Core.Module.ActivityService.Handler;
using Cinnamon.Core.Module.UploadService.Handler;

namespace Cinnamon.Core.Module.CinnamonMakerService.Handler.Activity;

public class SubmitUpdatedActivityHandler : ISubmitUpdatedActivity 
{
    private readonly IUpdateActivityHandler updateActivityHandler;
    private readonly IUploadImages uploadImagesHandler;

    public SubmitUpdatedActivityHandler(IUpdateActivityHandler updateActivityHandler,
        IUploadImages uploadImagesHandler)
    {
        this.updateActivityHandler = updateActivityHandler;
        this.uploadImagesHandler = uploadImagesHandler;
    }

    public AppResult<SubmitUpdatedActivityResult> Execute(SubmitUpdatedActivity args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<SubmitUpdatedActivityResult>.CreateFailed(ex, "An error occured in SubmitUpdatedActivityHandler");
        }
    }

    public async Task<AppResult<SubmitUpdatedActivityResult>> ExecuteAsync(SubmitUpdatedActivity args)
    {
        try 
        {
            // check first if activity exist
            var activity = await CoreDI.DataStore.Activities.GetActivityByIdAsync(args.Activity.Id);
            if(activity == null)
            {
                return AppResult<SubmitUpdatedActivityResult>.CreateFailed(new ApplicationException("Can't find activity to update"), "Can't find activity to update");
            }

            // validate search tags
            if(args.SearchTags.Count == 0)
            {
                return AppResult<SubmitUpdatedActivityResult>.CreateFailed(new ApplicationException("Should have atleast 1 search tag"), "Should have atleast 1 search tag");
            }

            if(!IsAllSearchTagsValid(args.SearchTags))
            {
                return AppResult<SubmitUpdatedActivityResult>.CreateFailed(new ApplicationException("Some search tag is invalid"), "Some search tag is invalid");
            }

            // validate activity images
            // if(args.Images.Count < 3)
            // {
            //     return AppResult<SubmitUpdatedActivityResult>.CreateFailed(new ApplicationException("Should have atleast 3 images"), "Should have atleast 3 images");
            // }

            // popluate searh tags
            if(args.Activity.SearchTagsModel == null)
            {
                args.Activity.SearchTagsModel = new SearchTagsModel();
            }
            if(args.SearchTags.Count > 0 )
            {
                args.Activity.SearchTagsModel.SearchTag1 = args.SearchTags.ElementAt(0).Item2;
            }
            else 
            {
                args.Activity.SearchTagsModel.SearchTag1 = null;
            }
            if(args.SearchTags.Count > 1)
            {
                args.Activity.SearchTagsModel.SearchTag2 = args.SearchTags.ElementAt(1).Item2;
            }
            else 
            {
                args.Activity.SearchTagsModel.SearchTag2 = null;
            }
            if(args.SearchTags.Count > 2)
            {
                args.Activity.SearchTagsModel.SearchTag3 = args.SearchTags.ElementAt(2).Item2;
            }
            else 
            {
                args.Activity.SearchTagsModel.SearchTag3 = null;
            }
            if(args.SearchTags.Count > 3)
            {
                args.Activity.SearchTagsModel.SearchTag4 = args.SearchTags.ElementAt(3).Item2;
            }
            else 
            {
                args.Activity.SearchTagsModel.SearchTag4 = null;
            }
            if(args.SearchTags.Count > 4)
            {
                args.Activity.SearchTagsModel.SearchTag5 = args.SearchTags.ElementAt(4).Item2;
            }
            else 
            {
                args.Activity.SearchTagsModel.SearchTag5 = null;
            }

            // upload images first
            IList<Tuple<byte[], string>> images = new List<Tuple<byte[], string>>();
            var ids = new List<Tuple<int,string>>();
            // upload only new images
            foreach(var item in args.Images.Where(i => i.Item2 != null))
            {
                images.Add(new Tuple<byte[], string>(item.Item2,item.Item3));
                ids.Add(new Tuple<int, string>(item.Item1,item.Item3));
            }

            var upload = await uploadImagesHandler.ExecuteAsync(
                new UploadService.Interactors.ImageUpload { Container = "upload-container", Images = images });
            if(!upload.Succeeded)
            {
                return AppResult<SubmitUpdatedActivityResult>.CreateFailed(upload.Error.Exception, upload.Message);
            }

            // for update only image location and image name
            foreach(var item in args.Activity.ActivityImages)
            {
                var t = ids.Find(i => i.Item1 == item.Id);
                if(t != null)
                {
                    item.ImageLocation = upload.Result.ImagesPath[ids.IndexOf(t)];
                    item.ImageName = t.Item2;
                }
            }

            // save updated activity
            var updatedActivity = await updateActivityHandler.ExecuteAsync(new ActivityService.Interactors.UpdateActivity {Activity = args.Activity});
            if(!updatedActivity.Succeeded)
            {
                return AppResult<SubmitUpdatedActivityResult>.CreateFailed(updatedActivity.Error.Exception, updatedActivity.Message);
            }

            return AppResult<SubmitUpdatedActivityResult>
                .CreateSucceeded(new SubmitUpdatedActivityResult 
                    {Activity = updatedActivity.Result.Activity}, "Activity successfully updated");
        }
        catch (Exception ex)
        {
            return AppResult<SubmitUpdatedActivityResult>.CreateFailed(ex, "An error occured in SubmitUpdatedActivityHandler");
        }
    }

    private bool IsAllSearchTagsValid(IList<Tuple<int,string>> tags)
    {
        foreach(var item in tags)
        {
            if(string.IsNullOrEmpty(item.Item2)) return false;
        }
        return true;
    }
}