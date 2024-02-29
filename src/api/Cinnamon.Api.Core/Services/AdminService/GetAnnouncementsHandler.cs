using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AdminService.Handlers;
using Cinnamon.Api.Core.Services.AdminService.Interactors;
using Cinnamon.Api.Core.Services.AdminService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AdminService;

public class GetAnnouncementsHandler : IGetAnnouncementsHandler
{
    private readonly IAnnouncementData announcementData;

    public GetAnnouncementsHandler(IAnnouncementData announcementData)
    {
        this.announcementData = announcementData;
    }

    public AppResult<GetAnnouncementsResult> Execute(GetAnnouncementsArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<GetAnnouncementsResult>> ExecuteAsync(GetAnnouncementsArgs args)
    {
        try
        {
            var announcementRes = await announcementData.GetAnnouncements();
            if(!announcementRes.Succeeded || announcementRes.Result is null || !announcementRes.Result.IsSuccess)
            {
                return AppResult<GetAnnouncementsResult>.CreateFailed(new ApplicationException(announcementRes.Result?.ErrorInfo?.Message), announcementRes.Message);
            }

            var announcements = announcementRes.Result.Result.Where(
                a => !string.IsNullOrEmpty(args.Status) ? a.Status.Equals(args.Status, StringComparison.CurrentCultureIgnoreCase) : true);
            
            return AppResult<GetAnnouncementsResult>.CreateSucceeded(new GetAnnouncementsResult {
                Announcements = announcements.Select(a => new GetAnnouncementsResult.Announcement {
                    ButtonLabel = a.ButtonLabel,
                    Description = a.Description,
                    Id = a.Id,
                    Link = a.Link,
                    Status = a.Status,
                    Title = a.Title
                })
            }, "Successfully get all announcemets.");
        }
        catch (Exception ex)
        {
            return AppResult<GetAnnouncementsResult>.CreateFailed(ex, "An error occured in GetAnnouncementsHandler.");
        }
    }
}