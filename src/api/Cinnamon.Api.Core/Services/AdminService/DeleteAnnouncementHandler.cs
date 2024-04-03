using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AdminService.Handlers;
using Cinnamon.Api.Core.Services.AdminService.Interactors;
using Cinnamon.Api.Core.Services.AdminService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AdminService;

public class DeleteAnnouncementHandler : IDeleteAnnouncementHandler
{
    private readonly IGetProfileHandler getProfileHandler;
    private readonly IGetAdminUserByEmailHandler getAdminUserByEmailHandler;
    private readonly IAnnouncementData announcementData;

    public DeleteAnnouncementHandler(IGetProfileHandler getProfileHandler, IGetAdminUserByEmailHandler getAdminUserByEmailHandler,
        IAnnouncementData announcementData)
    {
        this.getAdminUserByEmailHandler = getAdminUserByEmailHandler;
        this.getProfileHandler = getProfileHandler;
        this.announcementData = announcementData;
    }
    
    public AppResult<DeleteAnnouncementResult> Execute(DeleteAnnouncementArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<DeleteAnnouncementResult>> ExecuteAsync(DeleteAnnouncementArgs args)
    {
        try
        {
            var getProfileRes = await getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs {});
            if(!getProfileRes.Succeeded || getProfileRes.Result is null)
            {
                return AppResult<DeleteAnnouncementResult>.CreateFailed(new ApplicationException("You are not allowed. Invalid request."), "You are not allowed. Invalid request.");
            }

            var getAdminRes = await getAdminUserByEmailHandler.ExecuteAsync(new AdminService.Interactors.GetAdminUserByEmailArgs {
                Email = getProfileRes.Result.Email
            });
            if(!getAdminRes.Succeeded || getAdminRes.Result is null)
            {
                return AppResult<DeleteAnnouncementResult>.CreateFailed(new ApplicationException("You are not allowed. Invalid request."), "You are not allowed. Invalid request.");
            }

            var deleteRes = await announcementData.DeleteAnnouncement(new Framework.ApiCommand.ApiData.Announcement.Request.DeleteAnnouncementArgs {
                Id = args.AnnouncementId
            });
            if(!deleteRes.Succeeded || deleteRes.Result is null || !deleteRes.Result.IsSuccess)
            {
                return AppResult<DeleteAnnouncementResult>.CreateFailed(new ApplicationException(deleteRes.Result?.ErrorInfo?.Message), deleteRes.Message);
            }

            return AppResult<DeleteAnnouncementResult>.CreateSucceeded(new DeleteAnnouncementResult {AnnouncementId = args.AnnouncementId}, "Successfully delete anouncement.");
        }
        catch (Exception ex)
        {
            
            return AppResult<DeleteAnnouncementResult>.CreateFailed(ex, "An error occured in DeleteAnnouncementHandler.");
        }
    }
}