using Cinnamon.Core.Common;
using Cinnamon.Core.Config;
using Cinnamon.Core.Module.NotificationService.Interactors;
using Cinnamon.Core.Module.NotificationService.Handler;
using Cinnamon.Core.Module.CinnamonMakerService.Interactors;
using Cinnamon.Core.Module.CinnamonMakerService.Interactors.Results;

namespace Cinnamon.Core.Module.CinnamonMakerService.Handler.WaitingList;

public class SubmitWaitingListHandler : ISubmitWaitngList 
{
    private readonly IEmailVerification emailVerification;
    private readonly CoreConfig coreConfig;

    public SubmitWaitingListHandler(IEmailVerification emailVerification, CoreConfig coreConfig)
    {
        this.emailVerification = emailVerification;
        this.coreConfig = coreConfig;
    }

    public AppResult<SubmitWaitingListResult> Execute(SubmitWaitingList args)
    {
        throw new NotImplementedException();
    }

    public async Task<AppResult<SubmitWaitingListResult>> ExecuteAsync(SubmitWaitingList args)
    {
        try
        {
            if(string.IsNullOrEmpty(args.Email))
            {
                return AppResult<SubmitWaitingListResult>.CreateFailed(new ApplicationException("Provide valid email"), "Provide valid email");
            }

            var guid = Guid.NewGuid();
            var timestamp = DateTime.UtcNow;

            // generate token
            byte[] time = BitConverter.GetBytes(timestamp.ToBinary());
            byte[] key = guid.ToByteArray();
            var token = Convert.ToBase64String(time.Concat(key).ToArray());

            var payload = new WaitListModel { Email = args.Email, IsVerified = false, Token = token, Type = args.UserType, Guid = guid.ToString(),
             AcceptFlag = false,Birthdate= String.Empty,ConfirmPassword= String.Empty,FirstName= String.Empty,LastName= String.Empty,Password= String.Empty
            };
            var waitingResult = await CoreDI.DataStore.WaitList.SaveDataAsync(payload);
            if(!waitingResult.Message.ToLower().Contains("saved"))
            {
                return AppResult<SubmitWaitingListResult>.CreateFailed(
                    new ApplicationException("An error occured while saving to waiting list"), "An error occured while saving to waiting list");
            }
            string localhost = "https://localhost:7213";
            var verificationLink = $"{localhost}/Confirm-Email/?userid={guid.ToString()}&token={token}&email={args.Email}";
            var emailRes = await emailVerification.ExecuteAsync(new EmailVerification { Email = args.Email, VerificationLink = verificationLink });
            if(!emailRes.Succeeded) 
            {
                return AppResult<SubmitWaitingListResult>.CreateFailed(emailRes.Error.Exception, emailRes.Message);
            }

            return AppResult<SubmitWaitingListResult>
                .CreateSucceeded(new SubmitWaitingListResult { GeneratedVerificationLink = verificationLink }, "Email successfully saved to waiting list");

        }
        catch (Exception ex)
        {
            return AppResult<SubmitWaitingListResult>.CreateFailed(ex, "An error occured during SubmitWaitingListHandler");
        }
    }
}