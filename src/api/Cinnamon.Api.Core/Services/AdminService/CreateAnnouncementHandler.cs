using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AdminService.Handlers;
using Cinnamon.Api.Core.Services.AdminService.Interactors;
using Cinnamon.Api.Core.Services.AdminService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AdminService;

public class CreateAnnouncementHandler : ICreateAnnouncementHandler
{
    private readonly IGetProfileHandler getProfileHandler;
    private readonly IGetAdminUserByEmailHandler getAdminUserByEmailHandler;
    private readonly IAnnouncementData announcementData;

    public CreateAnnouncementHandler(IGetProfileHandler getProfileHandler, IGetAdminUserByEmailHandler getAdminUserByEmailHandler,
        IAnnouncementData announcementData)
    {
        this.getAdminUserByEmailHandler = getAdminUserByEmailHandler;
        this.getProfileHandler = getProfileHandler;
        this.announcementData = announcementData;
    }
    
    public AppResult<CreateAnnouncementResult> Execute(CreateAnnouncementArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<CreateAnnouncementResult>> ExecuteAsync(CreateAnnouncementArgs args)
    {
        try
        {
            var allowedStatus = new string[] {"draft", "published"};
            if(!allowedStatus.Any(s => s.Equals(args.Status, StringComparison.CurrentCultureIgnoreCase)))
            {
                return AppResult<CreateAnnouncementResult>.CreateFailed(new ApplicationException("Invalid status. Invalid request."), "Invalid status. Invalid request.");
            }

            var getProfileRes = await getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs {});
            if(!getProfileRes.Succeeded || getProfileRes.Result is null)
            {
                return AppResult<CreateAnnouncementResult>.CreateFailed(new ApplicationException("You are not allowed. Invalid request."), "You are not allowed. Invalid request.");
            }

            var getAdminRes = await getAdminUserByEmailHandler.ExecuteAsync(new AdminService.Interactors.GetAdminUserByEmailArgs {
                Email = getProfileRes.Result.Email
            });
            if(!getAdminRes.Succeeded || getAdminRes.Result is null)
            {
                return AppResult<CreateAnnouncementResult>.CreateFailed(new ApplicationException("You are not allowed. Invalid request."), "You are not allowed. Invalid request.");
            }

            var createAnnnouncementRes = await announcementData.CreateAnnouncement(new Framework.ApiCommand.ApiData.Announcement.Request.CreateAnnouncementArgs {
                AdminId = getAdminRes.Result.AdminUserDetail.Id,
                ButtonLabel = args.ButtonLabel,
                Description = args.Description,
                Link = args.Link,
                Status = args.Status,
                Title = args.Title
            });
            if(!createAnnnouncementRes.Succeeded || createAnnnouncementRes.Result is null || !createAnnnouncementRes.Result.IsSuccess)
            {
                return AppResult<CreateAnnouncementResult>.CreateFailed(
                    new ApplicationException(createAnnnouncementRes.Result?.ErrorInfo?.Message), createAnnnouncementRes.Message);
            }
            var result = createAnnnouncementRes.Result.Result;

            return AppResult<CreateAnnouncementResult>.CreateSucceeded(new CreateAnnouncementResult {
                ButtonLabel = result.ButtonLabel,
                Description = result.Description,
                Id = result.Id,
                Link = result.Link,
                Status = result.Status,
                Title = result.Title
            }, "Successfully create announcement.");
        }
        catch (Exception ex)
        {
            return AppResult<CreateAnnouncementResult>.CreateFailed(ex, "An error occured in CreateAnnouncementHandler.");
        }
    }
}