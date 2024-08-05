using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Cinnamon.Api.Core.Config;
using Cinnamon.Framework.Common;
using Cinnamon.Api.Core.Modules.NotificationDriver.Handler;
using Cinnamon.Api.Core.Modules.NotificationDriver.Interactors.Results;
using Cinnamon.Api.Core.Modules.NotificationDriver.Interactors;

namespace Cinnamon.Api.Core.Modules.NotificationDriver.MailChimp;

public class MailchimpServices : IMailchimpServices
{
    private readonly IFlurlClient _flurlClient;
    private readonly ILogger _logger;
    private readonly string _apiKey;
    private readonly string _audienceId;
    private readonly string _dataCenter;
    public MailchimpServices(IFlurlClientFactory flurlFac, ApplicationConfig config, ILogger<MailchimpServices> logger)
    {
        _apiKey = config.MailChimpConfig.ApiKey;
        _audienceId = config.MailChimpConfig.AudienceId;
        _dataCenter = _apiKey.Split('-')[1];
        _flurlClient = flurlFac.Get($"https://{_dataCenter}.api.mailchimp.com/3.0/");
        _logger = logger;
    }

    public AppResult<MailchimpResult> Execute(MailchimpArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<MailchimpResult>.CreateFailed(ex, "An error occured in OteCustomerPayedNotificationHandler");
        }
    }

    public async Task<AppResult<MailchimpResult>> ExecuteAsync(MailchimpArgs args)
    {
        try
        {
            var url = $"lists/{_audienceId}/members/";
            var data = new
            {
                email_address = args.Email,
                status = "subscribed"
            };
            
            var response = await _flurlClient
                .Request(url)
                .WithBasicAuth("anystring", _apiKey)
                .PostJsonAsync(data);
            if (response.StatusCode == 200)
            {
                var result = await response.GetJsonAsync<MailchimpResult>();
                return AppResult<MailchimpResult>.CreateSucceeded(result, "Subscription is successful");
            }
            else
            {
                return AppResult<MailchimpResult>.CreateFailed(new Exception("Failed to subscribe user"), "Failed to subscribe user.");
            }
        }
        catch (FlurlHttpException ex)
        {
            _logger.LogError($"Error subscribing user to Mailchimp: {ex.Message}");
            var error = await ex.GetResponseJsonAsync();
            return AppResult<MailchimpResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred when subscribing user to Mailchimp: {ex.Message}");
            return AppResult<MailchimpResult>.CreateFailed(ex, "An error occurred when subscribing user to Mailchimp.");
        }
    }
}