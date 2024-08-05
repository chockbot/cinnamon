using Cinnamon.Framework.ApiCommand.ApiCore.System.Response;
using Cinnamon.Framework.Common;
using Cinnamon.Web.Modules.ApiAccess.Handlers;
using Flurl.Http;
using Flurl.Http.Configuration;
using Cinnamon.Framework.ApiCommand.ApiCore.System.Request;
using NuGet.Common;

namespace Cinnamon.Web.Modules.ApiAccess.System;

public class SystemApiHandler: ISystemApiHandler
{
    private readonly IFlurlClient flurlClient;

	public SystemApiHandler(IFlurlClientFactory flurlFac, Config.Config config)
	{
        flurlClient = flurlFac.Get(config.ApiUrl);
    }

    public async Task<AppResult<GetServerDateResult>> GetServerDate()
    {
        try
        {
            var result = await flurlClient
                .Request("System/GetServerDate")
                .GetJsonAsync<GetServerDateResult>();

            return AppResult<GetServerDateResult>.CreateSucceeded(result, "Successfully getting server date api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetServerDateResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetServerDateResult>.CreateFailed(ex, "An error occured when getting server date api");
        }
    }

    public async Task<AppResult<GetAnnouncementsResult>> GetAnnouncements()
    {
        try
        {
            var result = await flurlClient
                .Request("System/GetAnnouncements")
                .GetJsonAsync<GetAnnouncementsResult>();

            return AppResult<GetAnnouncementsResult>.CreateSucceeded(result, "Successfully get announcement api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAnnouncementsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAnnouncementsResult>.CreateFailed(ex, "An error occured when get announcement api");
        }
    }

    public async Task<AppResult<GetEventPoliciesResult>> GetEventPolicies()
    {
        try
        {
            var result = await flurlClient
                .Request("System/GetEventPolicies")
                .GetJsonAsync<GetEventPoliciesResult>();

            return AppResult<GetEventPoliciesResult>.CreateSucceeded(result, "Successfully getting event policies content.");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetEventPoliciesResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetEventPoliciesResult>.CreateFailed(ex, "An error occured when getting event policies content.");
        }
    }

    public async Task<AppResult<GetEventBuyerPoliciesResult>> GetEventBuyerPolicies()
    {
        try
        {
            var result = await flurlClient
                .Request("System/GetEventBuyerPolicies")
                .GetJsonAsync<GetEventBuyerPoliciesResult>();

            return AppResult<GetEventBuyerPoliciesResult>.CreateSucceeded(result, "Successfully getting event policies content.");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetEventBuyerPoliciesResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetEventBuyerPoliciesResult>.CreateFailed(ex, "An error occured when getting event policies content.");
        }
    }

    public async Task<AppResult<GetEventSellerPoliciesResult>> GetEventSellerPolicies()
    {
        try
        {
            var result = await flurlClient
                .Request("System/GetEventSellerPolicies")
                .GetJsonAsync<GetEventSellerPoliciesResult>();

            return AppResult<GetEventSellerPoliciesResult>.CreateSucceeded(result, "Successfully getting event policies content.");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetEventSellerPoliciesResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetEventSellerPoliciesResult>.CreateFailed(ex, "An error occured when getting event policies content.");
        }
    }

    public async Task<AppResult<GetPrivacyPoliciesResult>> GetPrivacyPolicies()
    {
        try
        {
            var result = await flurlClient
                .Request("System/GetPrivacyPolicies")
                .GetJsonAsync<GetPrivacyPoliciesResult>();

            return AppResult<GetPrivacyPoliciesResult>.CreateSucceeded(result, "Successfully getting event policies content.");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetPrivacyPoliciesResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetPrivacyPoliciesResult>.CreateFailed(ex, "An error occured when getting event policies content.");
        }
    }

    public async Task<AppResult<SubscribeToMailchimpResult>> SubscribeToMailchimp(SubscribeToMailchimpArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("System/SubcribeToMailchimp")
                .PostJsonAsync(args)
                .ReceiveJson<SubscribeToMailchimpResult>();

            return AppResult<SubscribeToMailchimpResult>.CreateSucceeded(result, "User has been subscribed to Mailchimp!");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<SubscribeToMailchimpResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<SubscribeToMailchimpResult>.CreateFailed(ex, "Failed subscribing user to mailchimp.");
        }
    }
}
