using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.Announcement.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Announcement.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.Announcement;

public class AnnouncementData : IAnnouncementData
{
    private readonly IFlurlClient flurlClient;

    public AnnouncementData(ApplicationConfig config, IFlurlClientFactory flurlFac)
    {
        flurlClient = flurlFac.Get(config.ApiDataUrl);
    }

    public async Task<AppResult<GetAnnouncementsResult>> GetAnnouncements()
    {
        try
        {
            var result = await flurlClient
                            .Request("Announcements")
                            .GetJsonAsync<GetAnnouncementsResult>();
            return AppResult<GetAnnouncementsResult>.CreateSucceeded(result, "Successfully get announcements api");
        }
        catch (FlurlHttpException ex)
        {
            var flurlError = await ex.GetResponseJsonAsync();
            return AppResult<GetAnnouncementsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAnnouncementsResult>.CreateFailed(ex, "An error occured when get announcements api");
        }
    }

    public async Task<AppResult<CreateAnnouncementResult>> CreateAnnouncement(CreateAnnouncementArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Announcements")
                            .PostJsonAsync(args)
                            .ReceiveJson<CreateAnnouncementResult>();
            return AppResult<CreateAnnouncementResult>.CreateSucceeded(result, "Successfully posting create announcement api");
        }
        catch (FlurlHttpException ex)
        {
            var flurlError = await ex.GetResponseJsonAsync();
            return AppResult<CreateAnnouncementResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateAnnouncementResult>.CreateFailed(ex, "An error occured when posting create announcement api");
        }
    }

    public async Task<AppResult<DeleteAnnouncementResult>> DeleteAnnouncement(DeleteAnnouncementArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Announcements/DeleteAnnouncement")
                            .PostJsonAsync(args)
                            .ReceiveJson<DeleteAnnouncementResult>();
            return AppResult<DeleteAnnouncementResult>.CreateSucceeded(result, "Successfully posting delete announcement api");
        }
        catch (FlurlHttpException ex)
        {
            var flurlError = await ex.GetResponseJsonAsync();
            return AppResult<DeleteAnnouncementResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<DeleteAnnouncementResult>.CreateFailed(ex, "An error occured when posting delete announcement api");
        }
    }

    public async Task<AppResult<UpdateAnnouncementResult>> UpdateAnnouncement(UpdateAnnouncementArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Announcements/UpdateAnnouncement")
                            .PostJsonAsync(args)
                            .ReceiveJson<UpdateAnnouncementResult>();
            return AppResult<UpdateAnnouncementResult>.CreateSucceeded(result, "Successfully posting update announcement api");
        }
        catch (FlurlHttpException ex)
        {
            var flurlError = await ex.GetResponseJsonAsync();
            return AppResult<UpdateAnnouncementResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateAnnouncementResult>.CreateFailed(ex, "An error occured when posting update announcement api");
        }
    }
}