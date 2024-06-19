using System.Net.Mail;
using Cinnamon.Api.Core.Modules.EmailDriver.Handlers;
using Cinnamon.Api.Core.Modules.EmailDriver.Interactors;
using Cinnamon.Api.Core.Modules.EmailDriver.Interactors.Results;
using Cinnamon.Framework.Common;
using Cinnamon.Api.Core.Config;
using Flurl;
using Flurl.Http;
using Cinnamon.Api.Core.Modules.EmailDriver.MicrosoftGraph.ResponseObject;
using System.Net;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.EmailDriver.MicrosoftGraph;

public class SendMailByMicrosoftGraph : ISendMailHandler
{
    private readonly ApplicationConfig applicationConfig;
    private readonly IFlurlClient flurlClientAccessToken;
    private readonly IFlurlClient flurlClientGraphRequest;

    public SendMailByMicrosoftGraph(ApplicationConfig applicationConfig, IFlurlClientFactory flurlFac)
    {
        this.applicationConfig = applicationConfig;
        this.flurlClientAccessToken = flurlFac.Get("https://login.microsoftonline.com");
        this.flurlClientGraphRequest = flurlFac.Get("https://graph.microsoft.com/v1.0/");
    }

    public AppResult<SendMailResult> Execute(SendMailArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<SendMailResult>.CreateFailed(ex, "Exception caught during SendMailHandler");
        }
    }

    public async Task<AppResult<SendMailResult>> ExecuteAsync(SendMailArgs args)
    {
        try
        {
            // validate email
            var emailsValid = IsAllEmailValid(args.Recipients.ToList());
            if(!emailsValid)
            {
                return AppResult<SendMailResult>.CreateFailed(new ApplicationException("Some email provided are invalid"), "Some email provided are invalid");
            }

            var tokenResult = await GetAccessToken();
            if(!tokenResult.Succeeded || tokenResult.Result == null)
            {
                return AppResult<SendMailResult>.CreateFailed(tokenResult.Error.Exception, tokenResult.Message);
            }

            var sendResult = await Send(tokenResult.Result, args);
            if(!sendResult.Succeeded)
            {
                return AppResult<SendMailResult>.CreateFailed(sendResult.Error.Exception, sendResult.Message);
            }

            return AppResult<SendMailResult>.CreateSucceeded(new SendMailResult { RecipientsCount = args.Recipients.Count() }, "Email successfully sent");
        }
        catch (Exception ex)
        {
            return AppResult<SendMailResult>.CreateFailed(ex, "Exception caught during SendMailHandler");
        }
    }

    private bool IsAllEmailValid(IList<string> emails)
    {
        try 
        {
            foreach(var email in emails)
            {
                string trimmedEmail = email.Trim();
                if(trimmedEmail.EndsWith(".")) return false;

                var addr = new MailAddress(email);
                if(addr.Address != trimmedEmail) return false;
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    private async Task<AppResult<string>> GetAccessToken() 
    {
        try 
        {
            var emailConfig = applicationConfig.EmailService;

            var result = await this.flurlClientAccessToken
                                    .Request($"{emailConfig.DirectoryTenantId}/oauth2/token")
                                    .PostUrlEncodedAsync(new {
                                        grant_type = emailConfig.GrantType,
                                        client_id = emailConfig.ClientId,
                                        client_secret = emailConfig.ClientSecret,
                                        resource = emailConfig.Resource
                                    })
                                    .ReceiveJson<TokenResponse>();

            return AppResult<string>.CreateSucceeded(result.access_token, "Token successfully generated");
        }
        catch (Exception ex) 
        {
            return AppResult<string>.CreateFailed(ex, "An error occured when getting microsoft graph token");
        }
    }

    private async Task<AppResult<bool>> Send(string token, SendMailArgs args)
    {
        try
        {
            // set body payload to send
            var payload = new BodyPayload();
            payload.message.subject = args.Subject;
            payload.message.body.contentType = args.ContentType;
            payload.message.body.content = args.Body;

            // construct recipients data
            var recipients = new List<BodyPayload.Recipients>();
            foreach(var item in args.Recipients)
            {
                recipients.Add(new BodyPayload.Recipients { emailAddress = new BodyPayload.EmailAddress { address = item} });
            }
            payload.message.toRecipients = recipients;

            // from data
            payload.message.from = new BodyPayload.From { emailAddress = new BodyPayload.EmailAddress { address = applicationConfig.EmailService.Email } };

            var result = await flurlClientGraphRequest
                                .Request($"users/{applicationConfig.EmailService.UserId}/sendMail")
                                .WithHeaders(new {
                                    Content_Type = "application/json"
                                })
                                .WithOAuthBearerToken(token)
                                .PostJsonAsync(payload);
            
            if(result.StatusCode != (int)HttpStatusCode.Accepted)
            {
                return AppResult<bool>.CreateFailed(new ApplicationException("An error occured when sending email"), "An error occured when sending email");
            }

            return AppResult<bool>.CreateSucceeded(true, "Mail successfully sent");
        }
        catch (Exception ex)
        {
            return AppResult<bool>.CreateFailed(ex, "An error occured when sending email");
        }
    }
}