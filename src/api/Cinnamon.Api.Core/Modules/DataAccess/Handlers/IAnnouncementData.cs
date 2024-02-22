using Cinnamon.Framework.ApiCommand.ApiData.Announcement.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Announcement.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface IAnnouncementData 
{
    Task<AppResult<GetAnnouncementsResult>> GetAnnouncements();

    Task<AppResult<CreateAnnouncementResult>> CreateDisbursements(CreateAnnouncementArgs args);

    Task<AppResult<DeleteAnnouncementResult>> DeleteAnnouncement(DeleteAnnouncementArgs args);

    Task<AppResult<UpdateAnnouncementResult>> UpdateAnnouncement(UpdateAnnouncementArgs args);
}