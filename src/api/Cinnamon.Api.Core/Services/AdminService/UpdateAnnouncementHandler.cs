using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AdminService.Handlers;
using Cinnamon.Api.Core.Services.AdminService.Interactors;
using Cinnamon.Api.Core.Services.AdminService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AdminService;

public class UpdateAnnouncementHandler : IUpdateAnnouncementHandler
{
    private readonly IGetProfileHandler getProfileHandler;
    private readonly IGetAdminUserByEmailHandler getAdminUserByEmailHandler;
    private readonly IAnnouncementData announcementData;

    public UpdateAnnouncementHandler(IGetProfileHandler getProfileHandler, IGetAdminUserByEmailHandler getAdminUserByEmailHandler,
        IAnnouncementData announcementData)
    {
        this.getAdminUserByEmailHandler = getAdminUserByEmailHandler;
        this.getProfileHandler = getProfileHandler;
        this.announcementData = announcementData;
    }
    
    public AppResult<UpdateAnnouncementResult> Execute(UpdateAnnouncementArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<UpdateAnnouncementResult>> ExecuteAsync(UpdateAnnouncementArgs args)
    {
        try
        {
            var allowedStatus = new string[] {"draft", "published"};
            if(!allowedStatus.Any(s => s.Equals(args.Status, StringComparison.CurrentCultureIgnoreCase)))
            {
                return AppResult<UpdateAnnouncementResult>.CreateFailed(new ApplicationException("Invalid status. Invalid request."), "Invalid status. Invalid request.");
            }

            var getProfileRes = await getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs {});
            if(!getProfileRes.Succeeded || getProfileRes.Result is null)
            {
                return AppResult<UpdateAnnouncementResult>.CreateFailed(new ApplicationException("You are not allowed. Invalid request."), "You are not allowed. Invalid request.");
            }

            var getAdminRes = await getAdminUserByEmailHandler.ExecuteAsync(new AdminService.Interactors.GetAdminUserByEmailArgs {
                Email = getProfileRes.Result.Email
            });
            if(!getAdminRes.Succeeded || getAdminRes.Result is null)
            {
                return AppResult<UpdateAnnouncementResult>.CreateFailed(new ApplicationException("You are not allowed. Invalid request."), "You are not allowed. Invalid request.");
            }

            var updateRes = await announcementData.UpdateAnnouncement(new Framework.ApiCommand.ApiData.Announcement.Request.UpdateAnnouncementArgs {
                AdminId = getAdminRes.Result.AdminUserDetail.Id,
                ButtonLabel = args.ButtonLabel,
                Description = args.Description,
                Id = args.Id,
                Link = args.Link,
                Status = args.Status,
                Title = args.Title
            });
            if(!updateRes.Succeeded || updateRes.Result is null || !updateRes.Result.IsSuccess)
            {
                return AppResult<UpdateAnnouncementResult>.CreateFailed(new ApplicationException(updateRes.Result?.ErrorInfo?.Message), updateRes.Message);
            }
            var result = updateRes.Result.Result;

            return AppResult<UpdateAnnouncementResult>.CreateSucceeded(new UpdateAnnouncementResult {
                ButtonLabel = result.ButtonLabel,
                Description = result.Description,
                Id = result.Id,
                Link = result.Link,
                Status = result.Status,
                Title = result.Title
            }, "Announcement successfully updated.");
        }
        catch (Exception ex)
        {
            return AppResult<UpdateAnnouncementResult>.CreateFailed(ex, "An error occured when updating announcement.");
        }
    }
}