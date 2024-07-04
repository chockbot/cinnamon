using System.Net;
using System.Net.Mail;
using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.EmailDriver.Handlers;
using Cinnamon.Api.Core.Modules.EmailDriver.Interactors;
using Cinnamon.Api.Core.Modules.EmailDriver.Interactors.Results;
using Cinnamon.Api.Core.Modules.EmailDriver.MicrosoftGraph.ResponseObject;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.EmailDriver.MicrosoftGraph;

public class SendInviteEventHandler : ISendInviteEventHandler
{
    private readonly ApplicationConfig applicationConfig;
    private readonly IFlurlClient flurlClientAccessToken;
    private readonly IFlurlClient flurlClientGraphRequest;

    public SendInviteEventHandler(ApplicationConfig applicationConfig, IFlurlClientFactory flurlFac)
    {
        this.applicationConfig = applicationConfig;
        this.flurlClientAccessToken = flurlFac.Get("https://login.microsoftonline.com");
        this.flurlClientGraphRequest = flurlFac.Get("https://graph.microsoft.com/v1.0/");
    }

    public AppResult<SendInviteEventResult> Execute(SendInviteEventArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<SendInviteEventResult>> ExecuteAsync(SendInviteEventArgs args)
    {
        try
        {
            // validate email
            var emailsValid = IsAllEmailValid(args.Attendees.Select(a => a.Email));
            if(!emailsValid)
            {
                return AppResult<SendInviteEventResult>.CreateFailed(new ApplicationException("Some email provided are invalid"), "Some email provided are invalid");
            }

            var tokenResult = await GetAccessToken();
            if(!tokenResult.Succeeded || tokenResult.Result == null)
            {
                return AppResult<SendInviteEventResult>.CreateFailed(tokenResult.Error.Exception, tokenResult.Message);
            }

            var payload = new {
                subject = args.Subject,
                body = new {
                    contentType = "HTML",
                    content = args.Content
                },
                start = new {
                    dateTime = args.DateStart.ToString("yyyy-MM-ddThh:mm:ss"),
                    timeZone = "Asia/Singapore"
                },
                end = new {
                    dateTime = args.DateEnd.ToString("yyyy-MM-ddThh:mm:ss"),
                    timeZone = "Asia/Singapore"
                },
                location = new {
                    displayName = args.Location
                },
                attendees = args.Attendees.Select(a => new {
                    emailAddress = new {
                        address = a.Email,
                        name = a.Name
                    },
                    type = "required"
                }),
                allowNewTimeProposals = false
            };

            var result = await flurlClientGraphRequest
                            .Request($"users/{applicationConfig.EmailService.UserId}/calendar/events")
                            .WithHeaders(new {
                                Content_Type = "application/json"
                            })
                            .WithOAuthBearerToken(tokenResult.Result)
                            .PostJsonAsync(payload);
            
            if(result.StatusCode != (int)HttpStatusCode.Created)
            {
                return AppResult<SendInviteEventResult>.CreateFailed(new ApplicationException("An error occured when creating invite event email"), "An error occured when creating invite event email");
            }

            return AppResult<SendInviteEventResult>.CreateSucceeded(new SendInviteEventResult {Success = true}, "Mail successfully sent");
        }
        catch (Exception ex)
        {
            return AppResult<SendInviteEventResult>.CreateFailed(ex, "An error occured in SendInviteEventHandler.");
        }
    }
    
    private bool IsAllEmailValid(IEnumerable<string> emails)
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
}