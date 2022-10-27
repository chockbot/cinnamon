using System.Text;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using Cinnamon.Core.Common;
using Cinnamon.Core.Config;
using Cinnamon.Core.Services;
using Cinnamon.Core.Module.EmailService.Interactors;
using Cinnamon.Core.Module.EmailService.Interactors.Results;
using Cinnamon.Core.Module.EmailService.Handler.MicrosoftGraph.ResponseObject;

namespace Cinnamon.Core.Module.EmailService.Handler.MicrosoftGraph;

public class SendMailHandler : ISendMailHandler 
{
    private readonly CoreConfig coreConfig;
    private readonly IJsonSerializationService jsonSerialization;
    public SendMailHandler(CoreConfig coreConfig, IJsonSerializationService jsonSerialization)
    {
        this.coreConfig = coreConfig;
        this.jsonSerialization = jsonSerialization;
    }

    public AppResult<SendMailResults> Execute(SendMail args)
    {
        try
        {
            // validate email
            var emailsValid = IsAllEmailValid(args.Recipients);
            if(!emailsValid)
            {
                return AppResult<SendMailResults>.CreateFailed(new ApplicationException("Some email provided are invalid"), "Some email provided are invalid");
            }

            var tokenResult = GetAccessToken();
            if(!tokenResult.Succeeded)
            {
                return AppResult<SendMailResults>.CreateFailed(tokenResult.Error.Exception, tokenResult.Message);
            }

            var sendResult = Send(tokenResult.Result, args);
            if(!sendResult.Succeeded)
            {
                return AppResult<SendMailResults>.CreateFailed(sendResult.Error.Exception, sendResult.Message);
            }

            return AppResult<SendMailResults>.CreateSucceeded(new SendMailResults { RecipientsCount = args.Recipients.Count }, "Email successfully sent");
        }
        catch(Exception ex)
        {
            return AppResult<SendMailResults>.CreateFailed(ex, "Exception caught during SendMailHandler");
        }
    }

    public Task<AppResult<SendMailResults>> ExecuteAsync(SendMail args)
    {
        return Task.Run(() => Execute(args));
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

    private AppResult<string> GetAccessToken() 
    {
        try 
        {
            var emailConfig = coreConfig.EmailService;
            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", emailConfig.GrantType),
                new KeyValuePair<string, string>("client_id", emailConfig.ClientId),
                new KeyValuePair<string, string>("client_secret", emailConfig.ClientSecret),
                new KeyValuePair<string, string>("resource", emailConfig.Resource),
            };

            var content = new FormUrlEncodedContent(pairs);
            using(var httpClient = new HttpClient())
            {
                string url = $"https://login.microsoftonline.com/{emailConfig.DirectoryTenantId}/oauth2/token";
                var response = httpClient.PostAsync(url, content).Result;

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return AppResult<string>.CreateFailed(new ApplicationException("Unable to get microsoft graph token"), "Unable to get microsoft graph token");
                }

                var responseContent =  response.Content.ReadAsStringAsync().Result;
                var tokenResponse = jsonSerialization.Deserialize<TokenResponse>(responseContent);

                return AppResult<string>.CreateSucceeded(tokenResponse.access_token, "Token successfully generated");
            }
        }
        catch (Exception ex) 
        {
            return AppResult<string>.CreateFailed(ex, "An error occured when getting microsoft graph token");
        }
    }

    private AppResult<bool> Send(string token, SendMail args)
    {
        try
        {
            // set body payload to send
            var payload = new BodyPayload();
            payload.message.subject = args.Subject;
            payload.message.body.contentType = "text";
            payload.message.body.content = args.Body;

            // construct recipients data
            var recipients = new List<BodyPayload.Recipients>();
            foreach(var item in args.Recipients)
            {
                recipients.Add(new BodyPayload.Recipients { emailAddress = new BodyPayload.EmailAddress { address = item} });
            }
            payload.message.toRecipients = recipients;

            // from data
            payload.message.from = new BodyPayload.From { emailAddress = new BodyPayload.EmailAddress { address = args.From } };

            var json = jsonSerialization.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            using(var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Authorization",$"Bearer {token}");

                string url = $"https://graph.microsoft.com/v1.0/users/{coreConfig.EmailService.UserId}/sendMail";
                var result = httpClient.PostAsync(url, content).Result;

                if(result.StatusCode != HttpStatusCode.Accepted)
                {
                    return AppResult<bool>.CreateFailed(new ApplicationException("An error occured when sending email"), "An error occured when sending email");
                }

                return AppResult<bool>.CreateSucceeded(true, "Mail successfully sent");
            }
        }
        catch (Exception ex)
        {
            return AppResult<bool>.CreateFailed(ex, "An error occured when sending email");
        }
    }
}