using Cinnamon.Api.Core.Modules.EmailDriver.Handlers;
using Cinnamon.Api.Core.Modules.NotificationDriver.Interactors.Results;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;
using System;
using static Cinnamon.Api.Core.Modules.EmailDriver.MicrosoftGraph.ResponseObject.BodyPayload;
using System.Net.Mail;
using System.Security.Claims;
using Cinnamon.Api.Core.Config;

namespace Cinnamon.Api.Core.Services.AccountService
{
    public class VerifyUserNotificationHandler : IVerifyUserNotificationHandler
    {
        private readonly ISendMailHandler sendMailHandler;
        private readonly ApplicationConfig applicationConfig;
        private readonly ILogger logger;

        public VerifyUserNotificationHandler(ISendMailHandler sendMailHandler, ApplicationConfig applicationConfig,
            ILogger<VerifyUserNotificationHandler> logger)
        {
            this.sendMailHandler = sendMailHandler;
            this.applicationConfig = applicationConfig;
            this.logger = logger;
        }
        public AppResult<VerifyUserNotificationResult> Execute(VerifyUserNotificationArgs args)
        {
            try
            {
                return ExecuteAsync(args).Result;
            }
            catch (Exception ex)
            {
                return AppResult<VerifyUserNotificationResult>.CreateFailed(ex, "An error occured in VerifyUserNotificationHandler");
            }
        }

        public async Task<AppResult<VerifyUserNotificationResult>> ExecuteAsync(VerifyUserNotificationArgs args)
        {
            try
            {
                var templateLocation = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "UserVerificationRequest.html");
                var properties = typeof(VerifyUserNotificationArgs).GetProperties().ToDictionary(x => x, x => x.GetType().GetProperties());

                using (StreamReader sr = new(templateLocation))
                {
                    var emailBody = sr.ReadToEnd();

                    foreach (var item in properties)
                        emailBody = emailBody.Replace("{" + item.Key.Name + "}", typeof(VerifyUserNotificationArgs).GetProperty(item.Key.Name)?.GetValue(args)?.ToString());

                    var sendMailResponse = await sendMailHandler
                        .ExecuteAsync(new Modules.EmailDriver.Interactors.SendMailArgs
                        {
                            Body = emailBody,
                            Recipients = new List<string> { applicationConfig.EmailService.AdminEmail },
                            Subject = "User Verification Request",
                            ContentType = "html",
                        });

                    if (!sendMailResponse.Succeeded)
                    {
                        return AppResult<VerifyUserNotificationResult>
                            .CreateFailed(new ApplicationException(sendMailResponse.Error.Description), sendMailResponse.Message);
                    }
                }

                return AppResult<VerifyUserNotificationResult>.CreateSucceeded(
                        new VerifyUserNotificationResult { }, "User Verification Request sent");
            }
            catch (Exception ex)
            {
                return AppResult<VerifyUserNotificationResult>.CreateFailed(ex, "An error occured in VerifyUserNotificationHandler");
            }
        }
    }
}
